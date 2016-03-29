﻿using System;

namespace Assets.Scripts.Utils
{
    public static class UidGenerator
    {
        public static readonly int StartUid = 500000;
        public static readonly int InvalidUid = StartUid - 1;
        private static int _nextUid = StartUid;

        public static int GetNewUid()
        {
            if (!IsValid(_nextUid))
            {
                throw new FormatException("UID pool depleted.");
            }
            return _nextUid++;
        }

        public static bool IsValid ( int uid ) {
            return uid >= StartUid;
        }
    }
}