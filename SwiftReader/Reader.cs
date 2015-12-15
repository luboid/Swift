using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class Reader : IDisposable
    {
        public static readonly int READ_BUFFER_SIZE = 4*1024;

        class ReaderState
        {
            public int CurrentIndex = 0;
            public int Index = 0;
            public int Pos = 0;// position in source file
            public int Line = 0;
            public int Deep = 0;
            public int SecondOpen = 0;
            public int StartPos = 0;
            public int SearchStart = 0;
            public bool Ongoing = false;
            public Section Container = null;
            public Section Current = null;
            public Stack<Section> Stack = new Stack<Section>(3);
            public Stack<Section> Messages = new Stack<Section>(2);

            public Section Pop(bool atEnd = false)
            {
                if (atEnd)
                {
                    // searchBuffer.Length throw exception if buffer is not empty???
                    // no close character for message to move it to stateReader.Messages
                    if (Deep != 0)
                    {
                        throw new NotFullyEnededMessageException(Index);
                    }
                    return Stack.Count > 0 ? Stack.Pop() : null;
                }
                else
                {
                    return Messages.Count > 0 ? Messages.Pop() : null;
                }
            }
        }

        bool readerOwher;
        TextReader reader;
        ReaderState readerState;
        StringBuilder buffer;
        Settings settings;

        private Reader(TextReader streamReader, bool owner = false, Settings settings = null)
        {
            this.settings = settings ?? Settings.CreateDefault();
            if (this.settings.BufferSize <= 0)
            {
                this.settings.BufferSize = READ_BUFFER_SIZE;
            }

            this.readerOwher = owner;
            this.reader = streamReader;
            this.readerState = new ReaderState();
            this.buffer = new StringBuilder(this.settings.BufferSize);
        }

        public async Task<Section> ReadAsync()
        {
            var m = readerState.Pop();
            if (m != null)
            {
                return m;
            }

            var buffer = new char[settings.BufferSize];
            var read = 0; var searchIndex = 0;
            do
            {
                read = await reader.ReadAsync(buffer, 0, buffer.Length);
                if (0 == read)
                {
                    break;
                }
                else
                {
                    this.buffer.Append(buffer, 0, read);
                }

                searchIndex = ReadBuffer();
                if (searchIndex > -1)
                {
                    this.buffer.Remove(0, searchIndex);
                    readerState.StartPos = this.buffer.Length;
                }
                else
                {
                    readerState.StartPos = 0;
                }

                if ((m = readerState.Pop()) != null)
                {
                    return m;
                }

            } while (read != 0);


            return readerState.Pop(true);
        }

        public Section Read()
        {
            var m = readerState.Pop();
            if (m != null)
            {
                return m;
            }

            var buffer = new char[settings.BufferSize];
            var read = 0; var searchIndex = 0;
            do
            {
                read = reader.Read(buffer, 0, buffer.Length);
                if (0 == read)
                {
                    break;
                }
                else
                {
                    this.buffer.Append(buffer, 0, read);
                }

                searchIndex = ReadBuffer();
                if (searchIndex > -1)
                {
                    this.buffer.Remove(0, searchIndex);
                    readerState.StartPos = this.buffer.Length;
                }
                else
                {
                    readerState.StartPos = 0;
                }

                if ((m = readerState.Pop()) != null)
                {
                    return m;
                }

            } while (read != 0);


            return readerState.Pop(true);
        }

        int ReadBuffer()
        {
            var currentPos = 0; var find = false; var key = (char)0;
            var charSet = settings.CharSet;
            var sets = charSet != null && charSet.ContiniusCharSets != null && charSet.ContiniusCharSets.Length != 0;
            var oths = charSet != null && charSet.OtherSymbols != null && charSet.OtherSymbols.Length != 0;
            var newLine = false;

            for (int i = readerState.StartPos, l = buffer.Length; i < l; i++)
            {
                if (buffer[i] == 10)
                {
                    readerState.Line += 1;
                }

                if (!readerState.Ongoing)
                {
                    if (readerState.SearchStart > settings.MaxCharactersToStart)
                    {
                        throw new CantFindMessageException(readerState.Pos);
                    }

                    if (buffer[i] == settings.Begin || buffer[i] == '{') // char with code 1 mark start of swift message
                    {
                        readerState.Ongoing = true;
                        currentPos = buffer[i] == '{' ? i : i + 1;
                        find = true;
                    }
                    ++readerState.SearchStart;
                }

                if (readerState.Ongoing)
                {
                    if (!(sets && charSet.ContiniusCharSets.Where(a => a.Begin <= buffer[i] && buffer[i] <= a.End).FirstOrDefault() == null) &&
                        !(oths && Array.IndexOf<char>(charSet.OtherSymbols, buffer[i]) == -1))
                    {
                        if ((buffer[i] == settings.Begin || buffer[i] == settings.End || buffer[i] == '$') && readerState.Deep == 0)
                            throw new InvalidCharcterException(readerState.Index, readerState.Line, readerState.Pos);
                    }

                    if (buffer[i] == 13)
                    {
                        if (newLine)
                            throw new InvalidNewLineException(readerState.Index, readerState.Line, readerState.Pos);

                        newLine = true;
                    }
                    else
                    {
                        if (newLine)
                        {
                            if (buffer[i] != 10)
                                throw new InvalidNewLineException(readerState.Index, readerState.Line, readerState.Pos);

                            newLine = false;
                        }
                    }

                    if (buffer[i] == '{') //начало на секция
                    {
                        ++readerState.Deep; // Някога проверка максиму 3
                        ++readerState.SecondOpen;
                        if (readerState.Deep > 2)
                        {
                            throw new TooDeepException(readerState.Index, readerState.Deep);
                        }

                        if (readerState.Container == null)
                        {
                            readerState.Current = readerState.Container = new Section() { StartPos = readerState.Pos };
                        }

                        readerState.Stack.Push(readerState.Current);

                        if (readerState.SecondOpen == 2)
                        {
                            readerState.Container.Append(buffer.ToString(currentPos, i - currentPos));
                            readerState.Current.Data = buffer.ToString(currentPos + 1, i - currentPos - 1);
                        }
                        find = true;
                        currentPos = i;

                        var current = new Section { StartPos = readerState.Pos };
                        readerState.Current.Sections.Add(current);
                        readerState.Current = current;

                        if (readerState.Container.Sections.Count > settings.MaxSections)
                            throw new TooManySectionsException(readerState.Index);
                    }
                    else
                    {
                        if (buffer[i] == '}') //край на секция
                        {
                            --readerState.Deep;
                            readerState.SecondOpen = 0;
                            readerState.Current.EndPos = readerState.Pos;
                            readerState.Current.Index = ++readerState.CurrentIndex;

                            if (readerState.Current.Data == null)
                            {
                                readerState.Current.Data = buffer.ToString(currentPos + 1, i - currentPos -1);
                            }

                            if (readerState.Deep == 0)
                            {
                                if (readerState.Current.BlockId.Length != 1)
                                {
                                    throw new InvalidSectionTypeException(readerState.Index, readerState.Current.BlockId, readerState.Current.StartPos, readerState.Current.EndPos);
                                }
                                else
                                {
                                    key = readerState.Current.BlockId[0];
                                    if (!(('1' <= key && key <= '5') || key == 'S'))
                                        throw new InvalidSectionTypeException(readerState.Index, readerState.Current.BlockId, readerState.Current.StartPos, readerState.Current.EndPos);
                                }
                            }
                            readerState.Container.Append(buffer.ToString(currentPos, i - currentPos + 1));
                            readerState.Container.EndPos = readerState.Current.EndPos;
                            readerState.Current = readerState.Stack.Pop();
                            currentPos = i + 1;
                            find = true;
                        }
                        else
                        {
                            if (buffer[i] == settings.End || buffer[i] == '$') // char with code 1 mark end of swift message
                            {
                                readerState.Ongoing = buffer[i] == '$';// разделител м/у две съобщения
                                readerState.SearchStart = 0;
                                readerState.Container.EndPos = readerState.Pos - 1;
                                readerState.Container.Index = ++readerState.Index;
                                readerState.Messages.Push(readerState.Container);

                                readerState.Container = null;
                                readerState.Current = null;
                                readerState.CurrentIndex = 0;
                                readerState.Stack.Clear();
                                currentPos = i + 1;
                                find = true;
                            }
                        }
                    }
                }
                else
                {
                    currentPos = i + 1;//ignore white space
                }

                ++readerState.Pos;//позиция в файла
            }
            return find ? currentPos : -1;
        }

        public static Reader Create(string path, Settings settings = null)
        {
            return new Reader(new StreamReader(path), true, settings);
        }

        public static Reader Create(TextReader streamReader, Settings settings = null)
        {
            return new Reader(streamReader, false, settings);
        }

        void IDisposable.Dispose()
        {
            if (this.readerOwher && null != this.reader)
                this.reader.Dispose();
        }
    }
}