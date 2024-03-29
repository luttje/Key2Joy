﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Key2Joy.Mapping.Actions.Logic;

public static class IdPool
{
    private static int nextId = 1;
    private static List<BaseId> ids = new();

    public class BaseId
    {
        public int Id { get; set; }
        public CancellationTokenSource Cancellation { get; set; }

        public BaseId()
        {
            this.Id = nextId++;

            ids.Add(this);
        }

        public BaseId(CancellationTokenSource cancellation)
            : this() => this.Cancellation = cancellation;

        public void Cancel() => this.Cancellation.Cancel();

        public override string ToString() => this.Id.ToString();
    }

    public class TimeoutId : BaseId
    {
        public TimeoutId()
            : base()
        { }

        public TimeoutId(CancellationTokenSource cancellation)
            : base(cancellation)
        { }
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

    public static IdType CreateNewId<IdType>(CancellationTokenSource cancellation) where IdType : BaseId => (IdType)Activator.CreateInstance(typeof(IdType), cancellation);

    public static void CancelAll()
    {
        foreach (var id in ids)
        {
            id.Cancel();
        }

        ids = new List<BaseId>();
    }
}
