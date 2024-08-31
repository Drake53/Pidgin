#if NETSTANDARD2_0
using System;
using System.Buffers;
using System.IO;

namespace Pidgin
{
    internal static class TextReaderExtensions
    {
        internal static int Read(this TextReader textReader, Span<char> destination)
        {
            var sharedBuffer = ArrayPool<char>.Shared.Rent(destination.Length);
            try
            {
                var offset = 0;
                var bytesToCopy = destination.Length;
                while (bytesToCopy > 0)
                {
                    var read = textReader.Read(sharedBuffer, offset, bytesToCopy);
                    if (read == 0)
                    {
                        break;
                    }

                    offset += read;
                    bytesToCopy -= read;
                }

                new ReadOnlySpan<char>(sharedBuffer, 0, offset).CopyTo(destination);

                return offset;
            }
            finally
            {
                ArrayPool<char>.Shared.Return(sharedBuffer);
            }
        }
    }
}
#endif
