using System;
using UniRx;

public class TimerModel : IDisposable
{
    private readonly ReactiveProperty<float> _gameTime = new(0f);

    public IReactiveProperty<float> GameTime => _gameTime;

    public void Dispose()
    {
        _gameTime?.Dispose();
    }
}
