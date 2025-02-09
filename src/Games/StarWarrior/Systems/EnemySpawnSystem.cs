// Original code dervied from:
// https://github.com/thelinuxlich/starwarrior_CSharp/blob/master/StarWarrior/StarWarrior/Systems/EnemySpawnSystem.cs

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnemySpawnSystem.cs" company="GAMADU.COM">
//     Copyright � 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   The enemy spawn system.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using StarWarrior.Components;

namespace StarWarrior.Systems
{
    public class EnemySpawnSystem : EntityUpdateSystem
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly EntityFactory _entityFactory;
        private readonly Random _random = new Random();
        private ComponentMapper<EnemyComponent> _enemyComponentMapper;

        private Bag<int> _enemies;

        public EnemySpawnSystem(GraphicsDevice graphicsDevice, EntityFactory entityFactory) : base(Aspect.One(typeof(EnemyComponent)))
        {
            _graphicsDevice = graphicsDevice;
            _entityFactory = entityFactory;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _enemyComponentMapper = mapperService.GetMapper<EnemyComponent>();
            _enemies = new Bag<int>();
        }

        // Keep track of how many enemies there are
        protected override void OnEntityAdded(int entityId)
        {
            if (_enemyComponentMapper.Get(entityId) != null)
                _enemies.Add(entityId);
        }
        protected override void OnEntityRemoved(int entityId)
        {
            if (_enemyComponentMapper.Get(entityId) != null)
                _enemies.Remove(entityId);
        }

        public override void Update(GameTime gameTime)
        {
            // Only allow 10 enemies
            if (_enemies.Count > 10)
                return;

            var viewport = _graphicsDevice.Viewport;
            var entity = _entityFactory.CreateEnemyShip();
            var transform = entity.Get<Transform2>();

            Vector2 position;
            position.X = _random.Next(viewport.Width);
            position.Y = _random.Next(400) + 50;
            transform.Position = position;

            var physics = entity.Get<PhysicsComponent>();
            physics.Speed = 0.05f;
            physics.Angle = _random.Next() % 2 == 0 ? 0 : 180;
        }
    }
}
