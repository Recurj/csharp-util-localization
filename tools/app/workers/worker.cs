using System.Threading;

namespace RJToolsApp.workers {
    abstract public class Worker {
        protected CancellationTokenSource _breakHandle = null;
        protected Thread _thread = null;
        public Worker() {
            _breakHandle = new CancellationTokenSource();
        }
        public void Start() {
            _thread = new Thread(DoWork);
            _thread.Start();
        }
        public bool IsRunning() => _thread.IsAlive;
        public void Stop() => _breakHandle.Cancel();
        abstract protected void DoWork();
    }
}
