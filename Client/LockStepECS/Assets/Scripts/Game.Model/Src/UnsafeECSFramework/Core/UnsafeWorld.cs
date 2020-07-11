using Lockstep.Game;
using Lockstep.Logging;
using Lockstep.Math;
using Lockstep.Util;
using NetMsg.Common;
using Lockstep.UnsafeECS;

namespace Lockstep.UnsafeECS.Game {
    public partial class UnsafeWorld : World {
        private Context _context;
        private Systems _systems;
        public UnsafeWorld(IServiceContainer services, object contextsObj, object logicFeatureObj){
            var contexts = contextsObj as Context;
            var logicFeature = logicFeatureObj as Systems;
            _context = contexts;
            _context._entityService = services.GetService<IEntityService>();
            _timeMachineService = services.GetService<ITimeMachineService>();
            _systems = new WorldSystems(_context, services, logicFeature);
            _context.DoAwake(_systems,services);
            Debug.Log("UnsafeWorld Constructer");
            //temp code
        }

        protected override void DoSimulateAwake(IServiceContainer serviceContainer, IManagerContainer mgrContainer){
            InitReference(serviceContainer, mgrContainer);
            DoAwake(serviceContainer);
            DoStart();
        }

        protected override void DoSimulateStart(){
            Debug.Log("DoSimulateStart");
            UnityEngine.Profiling.Profiler.BeginSample("UnsafeECS DoInitialize");
            _context.DoInitialize();
            UnityEngine.Profiling.Profiler.EndSample();
        }

        protected override void DoStep(bool isNeedGenSnap){
            UnityEngine.Profiling.Profiler.BeginSample("UnsafeECS Update");
            _context.DoUpdate();
            UnityEngine.Profiling.Profiler.EndSample();
        }

        protected override void DoBackup(int tick){
            UnityEngine.Profiling.Profiler.BeginSample("_context.Backup");
            _context.Backup(tick);
            UnityEngine.Profiling.Profiler.EndSample();
            //Profiler.BeginSample("_context.DoCleanUselessSnapshot");
            //DoCleanUselessSnapshot(tick -1);
            //Profiler.EndSample();
        }
        protected override void DoRollbackTo(int tick, int missFrameTick, bool isNeedClear = true){
            _context.RollbackTo(tick,missFrameTick,isNeedClear);
        }
        protected override void DoCleanUselessSnapshot(int checkedTick){
            _context.CleanUselessSnapshot(checkedTick);
        }
        protected override void DoProcessInputQueue(byte actorId, InputCmd cmd){
            _context.ProcessInputQueue(actorId,cmd);
        }

    }
}