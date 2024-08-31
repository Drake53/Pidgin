#if NETSTANDARD2_0
using System;
using System.Buffers;
using System.IO;

namespace Pidgin
{
    internal static class StreamExtensions
    {
        internal static int Read(this Stream stream, Span<byte> destination)
        {
            var sharedBuffer = ArrayPool<byte>.Shared.Rent(destination.Length);
            try
            {
                var offset = 0;
                var bytesToCopy = destination.Length;
                while (bytesToCopy > 0)
                {
                    var read = stream.Read(sharedBuffer, offset, bytesToCopy);
                    if (read == 0)
                    {
                        break;
                    }

                    offset += read;
                    bytesToCopy -= read;
                }

                new ReadOnlySpan<byte>(sharedBuffer, 0, offset).CopyTo(destination);

                return offset;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(sharedBuffer);
            }
        }
    }
}
#endif
