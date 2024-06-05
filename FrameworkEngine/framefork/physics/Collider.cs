using Box2DX.Collision;
using Box2DX.Dynamics;
using Bubla;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFramework.framefork.physics
{
    public class Collider
    {
        private Body body;
        private PolygonDef shape;
        private CircleDef shapeCircle;
        private RectangleShape bodyDebugSquare;
        private SFML.Graphics.CircleShape bodyDebugCircle;
        private bool active;
        private float mass;
        private string typeCollider;

        public Collider(Vector2f size, Vector2f pos, float mass, bool fixedRotation, float rotate, string typeCollider, bool trigger)
        {
            if (!trigger) {
                active = true;
                this.mass = mass;
                this.typeCollider = typeCollider;

                BodyDef bDef = new BodyDef();
                bDef.Position.Set(pos.X, pos.Y);
                bDef.Angle = rotate;
                bDef.FixedRotation = fixedRotation;

                switch (typeCollider)
                {
                    case "square":
                        PolygonDef pDef = new PolygonDef();
                        pDef.Friction = 0.3f;
                        pDef.Density = 1;
                        pDef.SetAsBox(size.X / 2, size.Y / 2);
                        shape = pDef;
                        break;
                    case "circle":
                        CircleDef pDefCircle = new CircleDef();
                        pDefCircle.Friction = 0.3f;
                        pDefCircle.Density = 1;
                        pDefCircle.Radius = size.X / 2;
                        shapeCircle = pDefCircle;
                        break;
                }

                body = Game.GetWorld().CreateBody(bDef);
                Console.WriteLine(Game.GetWorld().GetBodyCount());
                if (shape != null) body.CreateShape(shape);
                else if (shapeCircle != null) body.CreateShape(shapeCircle);
                if (mass > 0) body.SetMassFromShapes();
                //Thread.Sleep(10);
            }

            //if (!Game.Debug) return;
            switch(typeCollider)
            {
                case "circle":
                    Vector2f sizeBodyDebugCircle = new Vector2f(size.X, size.Y);
                    bodyDebugCircle = new SFML.Graphics.CircleShape();
                    bodyDebugCircle.OutlineThickness = 3;
                    bodyDebugCircle.OutlineColor = Game.Debug ? SFML.Graphics.Color.Green : new SFML.Graphics.Color(0, 0, 0, 0);
                    bodyDebugCircle.FillColor = new SFML.Graphics.Color(0, 0, 0, 0);
                    bodyDebugCircle.Position = pos;
                    bodyDebugCircle.Radius = sizeBodyDebugCircle.X / 2;
                    bodyDebugCircle.Origin = new Vector2f(sizeBodyDebugCircle.X / 2, sizeBodyDebugCircle.Y / 2);
                    break;
                default: // = square
                    Vector2f sizeBodyDebugSquare = new Vector2f(size.X, size.Y);
                    bodyDebugSquare = new RectangleShape();
                    bodyDebugSquare.OutlineThickness = 3;
                    bodyDebugSquare.OutlineColor = Game.Debug ? SFML.Graphics.Color.Blue : new SFML.Graphics.Color(0, 0, 0, 0);
                    bodyDebugSquare.FillColor = new SFML.Graphics.Color(0, 0, 0, 0);
                    bodyDebugSquare.Position = pos;
                    bodyDebugSquare.Size = new Vector2f(sizeBodyDebugSquare.X, sizeBodyDebugSquare.Y);
                    bodyDebugSquare.Origin = new Vector2f(sizeBodyDebugSquare.X / 2, sizeBodyDebugSquare.Y / 2);
                    break;
            }
        }

        public SFML.Graphics.Shape Sprite
        {
            get {
                switch (typeCollider)
                {
                    case "circle":
                        return bodyDebugCircle;
                    default: // = square
                        return bodyDebugSquare;
                }
            }
        }

        public void SetPosition(Vector2f pos)
        {
            body.GetPosition().Set(pos.X, pos.Y);
        }

        public Body GetBody()
        {
            return body;
        }

        public void Impulse(float x, float y)
        {
            if (mass <= 0) return;
            body.SetLinearVelocity(new Box2DX.Common.Vec2(x, y * -1));
        }

        public void ImpulseX(float x)
        {
            if (mass <= 0) return;
            body.SetLinearVelocity(new Box2DX.Common.Vec2(x, body.GetLinearVelocity().Y));
        }

        public void ImpulseY(float y)
        {
            if (mass <= 0) return;
            body.SetLinearVelocity(new Box2DX.Common.Vec2(body.GetLinearVelocity().X, y * -1));
        }

        public void ApplyImpulse(float x, float y)
        {
            if (mass <= 0) return;
            body.ApplyForce(new Box2DX.Common.Vec2(x, y * -1), body.GetPosition());
        }

        public void Stop()
        {
            body.SetLinearVelocity(new Box2DX.Common.Vec2(0, 0));
        }

        public Box2DX.Common.Vec2 GetPosition()
        {
            return body.GetPosition();
        }

        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                if (value)
                {
                    body.CreateShape(shape);
                    bodyDebugSquare.OutlineColor = SFML.Graphics.Color.Green;
                }
                else
                {
                    body.DestroyShape(body.GetShapeList());
                    bodyDebugSquare.OutlineColor = new SFML.Graphics.Color(0, 150, 0);
                }
            }
        }

        public void Dispose()
        {
            if(bodyDebugSquare != null) bodyDebugSquare.Dispose();
            if(bodyDebugCircle != null) bodyDebugCircle.Dispose();
            //Console.WriteLine("asd " + (body.GetShapeList() == null));
            if (body != null) {
                if (body.GetShapeList() != null) body.DestroyShape(body.GetShapeList());
                body.Dispose();
            }
        }
    }
}
