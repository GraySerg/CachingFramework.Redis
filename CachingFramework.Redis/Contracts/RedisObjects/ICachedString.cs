﻿using System.Collections.Generic;

namespace CachingFramework.Redis.Contracts.RedisObjects
{
    /// <summary>
    /// Managed string using a Redis string.
    /// Redis Strings are limited to 512 megabytes
    /// </summary>
    public interface ICachedString : IEnumerable<byte>
    {
        /// <summary>
        /// Overwrites part of the string stored at key, starting at the specified offset, for the entire length of value. 
        /// If the offset is larger than the current length of the string at key, the string is padded with zero-bytes to make offset fit. 
        /// Non-existing keys are considered as empty strings, so this command will make sure it holds a string large enough to be able to set value at offset.
        /// </summary>
        /// <param name="offset">The zero-based offset in bytes.</param>
        /// <param name="item">The string to write.</param>
        /// <returns>The length of the string after it was modified by the command</returns>
        long SetRange(long offset, string item);
        /// <summary>
        /// Returns the substring of the string value stored at key, determined by the offsets start and stop (both are inclusive). 
        /// Negative offsets can be used in order to provide an offset starting from the end of the string. So -1 means the last character.
        /// </summary>
        /// <param name="start">The start zero-based index (can be negative number indicating offset from the end of the sorted set).</param>
        /// <param name="stop">The stop zero-based index (can be negative number indicating offset from the end of the sorted set).</param>
        string GetRange(long start = 0, long stop = -1);
        /// <summary>
        /// Returns the length of the string value stored at key.
        /// </summary>
        long Length { get; }
        /// <summary>
        /// Returns the substring of the string value stored at key, determined by the offsets start and stop (both are inclusive). 
        /// Negative offsets can be used in order to provide an offset starting from the end of the string. So -1 means the last character.
        /// </summary>
        /// <param name="start">The start zero-based index (can be negative number indicating offset from the end of the sorted set).</param>
        /// <param name="stop">The stop zero-based index (can be negative number indicating offset from the end of the sorted set).</param>
        string this[long start, long stop] { get; }
    }
}
