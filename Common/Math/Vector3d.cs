namespace Common.Math
{
    public struct Vector3d
    {
        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3d Zero = new Vector3d();

        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double LengthSquared => X * X + Y * Y + Z * Z;
        public double Length => System.Math.Sqrt(LengthSquared);
    }
}
