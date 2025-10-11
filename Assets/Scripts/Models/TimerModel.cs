using System;
using UniRx;

public class TimerModel : IDisposable
{
    public ReactiveProperty<float> GameTime = new(0f);

    public void Dispose()
    {
        GameTime?.Dispose();
    }
}