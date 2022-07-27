using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WaveFunctionCollapse
{
    public interface IValue<T> : IEqualityComparer<IValue<T>>, IEquatable<IValue<T>>
    {
        T Value { get; }
    }

}
