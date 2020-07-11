// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;

namespace Lockstep.UnsafeECSDefine {


    /// Component in game
    public interface IGameComponent : IComponent { }
    /// Service using in game
    public interface IGameService{}
    
    /// Game status which need read from config
    public interface IGameConfigService{}
    /// Game status which can not be modified in game
    public interface IGameConstStateService{}
    /// Game status can changed during game 
    public interface IGameStateService{}
    
    
    /// Create a game object to bind Entity
    /// to view the Entity's status or Attach some effect to the gameObject
    /// reference to :CodeGen_EntityView.cs
    public interface IBindViewEntity{}
    /// synchronize Unsafe Entity's Position and Rotation to Unity Entity
    /// reference to :CodeGen_UpdateViewStateSystem.cs
    public interface IUpdateViewEntity{}
}