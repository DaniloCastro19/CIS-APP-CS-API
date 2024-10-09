using System;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

public interface IBuilder<T, B>
{
    public B Build(T entity);

}
