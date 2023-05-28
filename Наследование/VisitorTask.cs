using Inheritance.Geometry.Virtual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Visitor
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }
        public abstract Body Accept(IVisitor iVisitor);
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }
        public override Body Accept(IVisitor iVisitor)  => iVisitor.Visit(this);

    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }
        public override Body Accept(IVisitor iVisitor) => iVisitor.Visit(this);
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }
        public override Body Accept(IVisitor iVisitor) => iVisitor.Visit(this);
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }
        public override Body Accept(IVisitor iVisitor) => iVisitor.Visit(this);
    }
    public interface IVisitor
    {
        Body Visit(Ball ball);
        Body Visit(RectangularCuboid rectangularCuboid);
        Body Visit(Cylinder cylinder);
        Body Visit(CompoundBody compoundBody);
    }
    public class BoundingBoxVisitor : IVisitor
    {
        public Body Visit(Ball ball)
        {
            var d = ball.Radius * 2;
            RectangularCuboid result = new RectangularCuboid(ball.Position, d, d, d);
            return result;
        }
        public Body Visit(RectangularCuboid rectangularCuboid)
        {
            RectangularCuboid result = new RectangularCuboid(rectangularCuboid.Position, rectangularCuboid.SizeX, rectangularCuboid.SizeY, rectangularCuboid.SizeZ);
            return result;
        }
        public Body Visit(Cylinder cylinder)
        {
            var d = cylinder.Radius * 2;
            RectangularCuboid result = new RectangularCuboid(cylinder.Position, d, d, cylinder.SizeZ);
            return result;
        }
            public Body Visit(CompoundBody compoundBody)
        {
            double minX = int.MaxValue;
            double minY = int.MaxValue;
            double minZ = int.MaxValue;
            double maxX = int.MinValue;
            double maxY = int.MinValue;
            double maxZ = int.MinValue;
            IEnumerable<RectangularCuboid> p = compoundBody.Parts.Select(part => (RectangularCuboid)part.Accept(this));
            Vector3 start = CalculateVector(ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ, p);
            RectangularCuboid result = new RectangularCuboid(start, Math.Abs(maxX - minX), Math.Abs(maxY - minY), Math.Abs(maxZ - minZ));
            return result;
        }

        private static Vector3 CalculateVector(ref double minX, ref double minY, ref double minZ, ref double maxX, ref double maxY, ref double maxZ, IEnumerable<RectangularCuboid> p)
        {
            foreach (var part in p)
            {
                double xStart = part.Position.X - part.SizeX / 2;
                double xEnd = xStart + part.SizeX;
                double yStart = part.Position.Y - part.SizeY / 2;
                double yEnd = yStart + part.SizeY;
                double zStart = part.Position.Z - part.SizeZ / 2;
                double zEnd = zStart + part.SizeZ;
                minX = Math.Min(minX, xStart);
                minY = Math.Min(minY, yStart);
                minZ = Math.Min(minZ, zStart);
                maxX = Math.Max(maxX, xEnd);
                maxY = Math.Max(maxY, yEnd);
                maxZ = Math.Max(maxZ, zEnd);
            }
            Vector3 start = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, (maxZ + minZ) / 2);
            return start;
        }
    }

    public class BoxifyVisitor : IVisitor
    {
        public Body Visit(Ball ball) => ball.Accept(new BoundingBoxVisitor());

        public Body Visit(RectangularCuboid rectangularCuboid) => rectangularCuboid;

        public Body Visit(Cylinder cylinder) => cylinder.Accept(new BoundingBoxVisitor());

        public Body Visit(CompoundBody compoundBody) => new CompoundBody(compoundBody.Parts.Select(part => part.Accept(this)).ToList());
    }
}