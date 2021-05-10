using System;
public interface IDestroyVfx
{
    public void PrepareDestroyVfx();
    public void TriggerDestroyVfx();
    public void OnDestroyVfxFinished();
}
