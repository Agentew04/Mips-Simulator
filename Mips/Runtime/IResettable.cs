namespace MipsSimulator.Mips.Runtime;

/// <summary>
/// Interface to signal that a component can be reseted
/// </summary>
public interface IResettable
{

    /// <summary>
    /// Resets all internal state of the component
    /// </summary>
    void Reset();
}
