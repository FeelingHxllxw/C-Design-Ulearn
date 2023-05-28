using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Virtual
{
    public abstract class Body
    {
        public Vector3 Position { get; }
        protected Body(Vector3 position)
        {
            Position = position;
        }
        public abstract bool ContainsPoint(Vector3 point);
        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }
        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= Radius * Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var d = Radius * 2;
            RectangularCuboid result = new RectangularCuboid(Position, d, d, d);
            return result;
        }
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

        public override bool ContainsPoint(Vector3 point)
        {
            var minPoint = new Vector3(
                Position.X - SizeX / 2,
                Position.Y - SizeY / 2,
                Position.Z - SizeZ / 2);
            var maxPoint = new Vector3(
                Position.X + SizeX / 2,
                Position.Y + SizeY / 2,
                Position.Z + SizeZ / 2);

            return point >= minPoint && point <= maxPoint;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            RectangularCuboid result = new RectangularCuboid(Position, SizeX, SizeY, SizeZ);
            return result;
        }
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

        public override bool ContainsPoint(Vector3 point)
        {
            var vectorX = point.X - Position.X;
            var vectorY = point.Y - Position.Y;
            var length2 = vectorX * vectorX + vectorY * vectorY;
            var minZ = Position.Z - SizeZ / 2;
            var maxZ = minZ + SizeZ;

            return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var d = Radius * 2;
            RectangularCuboid result = new RectangularCuboid(Position, d, d, SizeZ);
            return result;
        }
    }

    public class CompoundBody : Body
    {
        double minX = int.MaxValue;
        double minY = int.MaxValue;
        double minZ = int.MaxValue;
        double maxX = int.MinValue;
        double maxY = int.MinValue;
        double maxZ = int.MinValue;
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return Parts.Any(body => body.ContainsPoint(point));
        }
        public override RectangularCuboid GetBoundingBox()
        {
            IEnumerable<RectangularCuboid> p = Parts.Select(part => part.GetBoundingBox());
            foreach (var part in p)
            {
                double xStart = part.Position.X - part.SizeX / 2;
                double xEnd = xStart + part.SizeX;
                double yStart = part.Position.Y - part.SizeY / 2;
                double yEnd = yStart + part.SizeY;
                double zStart = part.Position.Z - part.SizeZ / 2;
                double zEnd = zStart + part.SizeZ;
                minX = Math.Min(minX, xStart);
                minY= Math.Min(minY, yStart);
                minZ = Math.Min(minZ, zStart);
                maxX = Math.Max(maxX, xEnd);
                maxY = Math.Max(maxY, yEnd);
                maxZ = Math.Max(maxZ, zEnd);
            }
            Vector3 start = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, (maxZ + minZ) / 2);
            RectangularCuboid result = new RectangularCuboid(start, Math.Abs(maxX - minX), Math.Abs(maxY - minY), Math.Abs(maxZ - minZ));
            return result;
        }
    }
}