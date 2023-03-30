using System;

namespace Entidades.Base
{
    public interface IBaseEntity<K> where K : IEquatable<K>
    {
        K Id { get; set; }
    }
}
