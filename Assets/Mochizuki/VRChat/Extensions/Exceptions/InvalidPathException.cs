/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;

namespace Mochizuki.VRChat.Extensions.Exceptions
{
    public class InvalidPathException : Exception
    {
        public InvalidPathException() { }

        public InvalidPathException(string message) : base(message) { }

        public InvalidPathException(string message, Exception innerException) : base(message, innerException) { }
    }
}