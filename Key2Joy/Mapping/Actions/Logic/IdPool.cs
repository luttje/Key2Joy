﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public static class IdPool
    {
        private static int nextId = 1;

        public class BaseId
        {
            public int Id { get; set; }
            public CancellationTokenSource Cancellation { get; set; }

            public BaseId()
            {
                Id = nextId++;
            }

            public BaseId(CancellationTokenSource cancellation)
                : this()
            {
                Cancellation = cancellation;
            }

            internal void Cancel()
            {
                Cancellation.Cancel();
            }

            override public string ToString()
            {
                return Id.ToString();
            }
        }

        public class TimeoutId : BaseId
        {
            public TimeoutId()
                : base()
            {}

            public TimeoutId(CancellationTokenSource cancellation)
                : base(cancellation)
            {}
        }

        public class IntervalId : BaseId
        {
            public IntervalId()
                : base()
            { }

            public IntervalId(CancellationTokenSource cancellation)
                : base(cancellation)
            { }
        }

        internal static IdType CreateNewId<IdType>(CancellationTokenSource cancellation) where IdType : BaseId
        {
            return (IdType)Activator.CreateInstance(typeof(IdType), cancellation);
        }
    }
}
