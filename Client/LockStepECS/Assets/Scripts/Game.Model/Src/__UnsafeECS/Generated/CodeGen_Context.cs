
//------------------------------------------------------------------------------    
// <auto-generated>                                                                 
//     This code was generated by Tools.MacroExpansion, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null. 
//     https://github.com/JiepengTan/LockstepEngine                                          
//     Changes to this file may cause incorrect behavior and will be lost if        
//     the code is regenerated.                                                     
// </auto-generated>                                                                
//------------------------------------------------------------------------------  

//Power by ME //src: https://github.com/JiepengTan/ME  

//#define DONT_USE_GENERATE_CODE                                                                 
                                                                                                 
using System.Linq;                                                                               
using Lockstep.Serialization;                                                                    
using System.Runtime.InteropServices;                                                            
using System.Runtime.CompilerServices;                                                            
using System;                                                                                    
using Lockstep.InternalUnsafeECS;                                                               
using System.Collections;                                                                        
using Lockstep.Math;                                                                             
using System.Collections.Generic;                                                                
using Lockstep.Logging;                                                                          
using Lockstep.Util;    
using Lockstep.UnsafeECS;
namespace Lockstep.UnsafeECS.Game {  
    using Lockstep.Game;    
    using NetMsg.Common;

    public unsafe partial class TempFields {
        private Context _context;
        public Dictionary<int, InputCmd> InputCmds = new Dictionary<int, InputCmd>();

        public TempFields(Context context){
            this._context = context;
        }
    }

    public unsafe partial class Context : BaseContext {
        private static Context _instance;
        public static Context Instance {
            get => _instance ?? (_instance = new Context());
            set => _instance = value;
        }
        public bool HasInit = false;
        public TempFields TempFields{ get;private set;}

        public _EntityManager _entities = new _EntityManager();
        public IEntityService _entityService;
        private IServiceContainer _services;
        public T GetService<T>() where T : Lockstep.Game.IService{
            if (_services == null) return default(T);
            return _services.GetService<T>();
        }


    #region Rollback Implement
        private ClassBackupHelper<_EntityManager> _entitiesBackuper = new ClassBackupHelper<_EntityManager>();

        protected override void _DoBackup(int tick){
            _entitiesBackuper.Backup(tick, _entities.Clone());
        }

        protected override void _DoRollbackTo(int tick, int missFrameTick, bool isNeedClear){
            var clone = _entitiesBackuper.RollbackTo(tick, missFrameTick, isNeedClear);
            clone.CopyTo(_entities);
        }

        protected override void _DoCleanUselessSnapshot(int checkedTick){
            _entitiesBackuper.CleanUselessSnapshot(checkedTick,(es)=>es.Free());
        }
    #endregion
    #region Lifecycle
        private FuncOnEntityCreated<PlayerCube> funcOnCreateEntityPlayerCube;
        private FuncOnEntityCreated<PlayerCube> funcResetEntityPlayerCube;

        protected override void _DoAwake(IServiceContainer services)
        {
            base._DoAwake(services);
            RegisterSystemFunctions();
            TempFields = new TempFields(this);
            OnInit(this,services);
            _entities.Alloc();
            _services = services;
            // reduce gc
            funcOnCreateEntityPlayerCube = OnEntityCreatedPlayerCube;
            funcResetEntityPlayerCube = ResetEntityPlayerCube; 
        }
        
        protected override void _DoDestroy(){
            TempFields.OnDestroy();
            OnDestroy();
            _entities.Free();
        }
        protected override void _BeforeSchedule(){
            TempFields.FramePrepare();
        }
        protected override void _AfterSchedule(){
            TempFields.FrameClearUp();
        }


        protected override void _DoDestroyEntity(EntityRef entityRef){
            DestroyEntityInternal(GetEntity(entityRef));
        }

        public Entity* GetEntity(EntityRef entityRef){
            switch (entityRef._type) {
                case EntityIds.PlayerCube: return (Entity*) GetPlayerCube(entityRef); 
            }
            return null;
        }

        private void DestroyEntityInternal(Entity* entity){
            if (entity == null) {
                return;
            }

            if (entity->_active == false) {
                return;
            }

            switch (entity->_ref._type) {
                case EntityIds.PlayerCube:
                    DestroyPlayerCubeInternal((PlayerCube*) entity);
                    break; 
            }
        }
  
        private unsafe void PostUpdateCreatePlayerCube(){
            _entities._PlayerCubeAry.PostUpdateCreate(funcOnCreateEntityPlayerCube,funcResetEntityPlayerCube);
        } 

    #endregion
    #region Entity PlayerCube
        private void OnEntityCreatedPlayerCube(PlayerCube* dstPtr){
            _EntityCreated(&dstPtr->_entity);
            _entityService.OnEntityCreated(this, (Entity*) dstPtr);
            _entityService.OnPlayerCubeCreated(this, dstPtr);
        }

        private void ResetEntityPlayerCube(PlayerCube* dstPtr){
            *dstPtr = _DefaultDefine.PlayerCube;
        }
        public Boolean PlayerCubeExists(EntityRef entityRef){
            return GetPlayerCube(entityRef) != null;
        }

        public PlayerCube* PostCmdCreatePlayerCube(){
            return _entities.CreateTempPlayerCube(this);
        }

        private void DestroyPlayerCubeInternal(PlayerCube* ptr){
            _entities._PlayerCubeAry.ReleaseEntity((Entity*)ptr);
            _entityService.OnPlayerCubeDestroy(this, ptr);
            _entityService.OnEntityDestroy(this, &ptr->_entity);
            var copy = ptr->_entity;
            *ptr = _DefaultDefine.PlayerCube;
            ptr->_entity = copy;
            _EntityDestroy(&ptr->_entity);
        }

        public void DestroyPlayerCube(PlayerCube* ptr){
            if (ptr == null) {
                return;
            }

            if (ptr->_entity._active == false) {
                return;
            }

            _destroy.Enqueue(ptr->EntityRef);
        }

        public void DestroyPlayerCube(EntityRef entityRef){
            _destroy.Enqueue(entityRef);
        }
    #endregion 

    #region GetEntity
        private PlayerCubeIterator GetAllPlayerCube(){
            return new PlayerCubeIterator(_entities.GetPlayerCube(0),_entities.MaxPlayerCubeIndex + 1);
        } 

        private EntityFilter[] GetAllEntities(){
            var all = new EntityFilter[_entities.CurTotalEntityCount];
            var count = 0;
            {
                var ptr = _entities.GetPlayerCube(0);
                var len = _entities._PlayerCubeAry.Length;
                for (var i = 0; i < len; ++i, ++ptr) {
                    all[count++].Entity = &ptr->_entity;
                }
            } 
            return all;
        }
    #endregion

    #region GetBuildInComponet
        public unsafe Buffer<AnimatorFilter> GetAllAnimator()
        {
            Buffer<AnimatorFilter> buffer = Buffer<AnimatorFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        }
        public unsafe Buffer<CollisionAgentFilter> GetAllCollisionAgent()
        {
            Buffer<CollisionAgentFilter> buffer = Buffer<CollisionAgentFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        }
        public unsafe Buffer<NavMeshAgentFilter> GetAllNavMeshAgent()
        {
            Buffer<NavMeshAgentFilter> buffer = Buffer<NavMeshAgentFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        }
        public unsafe Buffer<PrefabFilter> GetAllPrefab()
        {
            Buffer<PrefabFilter> buffer = Buffer<PrefabFilter>.Alloc(_entities.CurTotalEntityCount);
            PlayerCube* PlayerCubePtr = this._entities.GetPlayerCube(0);
            var idxPlayerCube = 2;
            while (idxPlayerCube >= 0)
            {
                if (PlayerCubePtr->_entity._active)
                {
                  buffer.Items[buffer.Count].Entity = &PlayerCubePtr->_entity;
                  buffer.Items[buffer.Count].Prefab = &PlayerCubePtr->Prefab;
                  ++buffer.Count;
                }
                --idxPlayerCube;
                ++PlayerCubePtr;
            } 
            return buffer;
        }
        public unsafe Buffer<Transform2DFilter> GetAllTransform2D()
        {
            Buffer<Transform2DFilter> buffer = Buffer<Transform2DFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        }
        public unsafe Buffer<Transform3DFilter> GetAllTransform3D()
        {
            Buffer<Transform3DFilter> buffer = Buffer<Transform3DFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        } 
    #endregion

    }
}                                                                                
                                                                                         