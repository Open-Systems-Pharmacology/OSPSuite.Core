using System;

namespace OSPSuite.Core.Maths
{
    public class Vector
    {
        private readonly double[] _components;
        private readonly int _nDimensions;

        public Vector(int dimensions)
        {
            _components = new double[dimensions];
            _nDimensions = dimensions;
        }

        public int NDimensions
        {
            get { return _nDimensions; }
        }

        public double this[int index]
        {
            get { return _components[index]; }
            set { _components[index] = value; }
        }

        public double[] Components
        {
            get { return _components; }
        }

        /// <summary>
        /// Add another vector to this one
        /// </summary>
        public Vector Add(Vector v)
        {
            if (v.NDimensions != this.NDimensions)
                throw new ArgumentException("Can only add vectors of the same dimensionality");

            Vector vector = new Vector(v.NDimensions);
            for (int i = 0; i < v.NDimensions; i++)
            {
                vector[i] = this[i] + v[i];
            }
            return vector;
        }

        /// <summary>
        /// Subtract another vector from this one
        /// </summary>
        public Vector Subtract(Vector v)
        {
            if (v.NDimensions != this.NDimensions)
                throw new ArgumentException("Can only subtract vectors of the same dimensionality");

            Vector vector = new Vector(v.NDimensions);
            for (int i = 0; i < v.NDimensions; i++)
            {
                vector[i] = this[i] - v[i];
            }
            return vector;
        }

        /// <summary>
        /// Multiply this vector by a scalar value
        /// </summary>
        public Vector Multiply(double scalar)
        {
            Vector scaledVector = new Vector(this.NDimensions);
            for (int i = 0; i < this.NDimensions; i++)
            {
                scaledVector[i] = this[i] * scalar;
            }
            return scaledVector;
        }

        /// <summary>
        /// Compute the dot product of this vector and the given vector
        /// </summary>
        public double DotProduct(Vector v)
        {
            if (v.NDimensions != this.NDimensions)
                throw new ArgumentException("Can only compute dot product for vectors of the same dimensionality");

            double sum = 0;
            for (int i = 0; i < v.NDimensions; i++)
            {
                sum += this[i] * v[i];
            }
            return sum;
        }

        public override string ToString()
        {
            string[] components = new string[_components.Length];
            for (int i = 0; i < components.Length; i++)
            {
                components[i] = _components[i].ToString();
            }
            return "[ " + string.Join(", ", components) + " ]";
        }
    }
}