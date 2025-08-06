using Services.Interfaces;
using Zenject;

namespace Services
{
    public class SceneUpdate : ITickable
    {
        [Inject]
        private IEnemyFactory _enemyFactory { get; set; }
        public void Tick()
        {
            _enemyFactory.Tick();
        }
    }
}
