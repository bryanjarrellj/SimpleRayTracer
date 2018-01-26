using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer {
    #region VECTOR3
    public class Vector3 {
        public double x, y, z;

        public Vector3() {
            x = y = z = 0;
        }

        public Vector3(double initVal) {
            x = y = z = initVal;
        }

        public Vector3(double xVal, double yVal, double zVal) {
            x = xVal;
            y = yVal;
            z = zVal;
        }

        public static double Dot(Vector3 lhs, Vector3 rhs) {
            return (lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z);
        }

        //cx = aybz − azby
        //cy = azbx − axbz
        //cz = axby − aybx
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs) {
            Vector3 vec = new Vector3();
            vec.x = lhs.y * rhs.z - lhs.z * rhs.y;
            vec.y = lhs.z * rhs.x - lhs.x * rhs.z;
            vec.z = lhs.x * rhs.y - lhs.y * rhs.x;
            return vec;
        }

        public void SetValues(double xVal, double yVal, double zVal) {
            x = xVal;
            y = yVal;
            z = zVal;
        }

        public void SetValues(double xVal, double yVal) {
            x = xVal;
            y = yVal;
        }

        public void AddVector(Vector3 otherVec) {
            this.x += otherVec.x;
            this.y += otherVec.y;
            this.z += otherVec.z;
        }

        public void Normalize() {
            double magValue = Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);

            this.x = this.x / magValue;
            this.y = this.y / magValue;
            this.z = this.z / magValue;
        }

        public static double Magnitude(Vector3 thisVec, Vector3 otherVec) {
            double x1 = (thisVec.x - otherVec.x) * (thisVec.x - otherVec.x);
            double y1 = (thisVec.y - otherVec.y) * (thisVec.y - otherVec.y);
            double z1 = (thisVec.z - otherVec.z) * (thisVec.z - otherVec.z);
            return Math.Sqrt(x1 + y1 + z1);
        }

        // Overload + operator
        public static Vector3 operator +(Vector3 b, Vector3 c) {
            Vector3 vec = new Vector3();
            vec.x = b.x + c.x;
            vec.y = b.y + c.y;
            vec.z = b.z + c.z;
            return vec;
        }

        //Subtract C FROM B
        public static Vector3 operator -(Vector3 b, Vector3 c) {
            Vector3 vec = new Vector3();
            vec.x = b.x - c.x;
            vec.y = b.y - c.y;
            vec.z = b.z - c.z;
            return vec;
        }

        //Subtract C FROM B
        public static Vector3 operator -(Vector3 a) {
            Vector3 vec = new Vector3();
            vec.x = a.x * -1;
            vec.y = a.x * -1;
            vec.z = a.z * -1;
            return vec;
        }

        public static Vector3 operator *(Vector3 b, double multNum) {
            Vector3 vec = new Vector3();
            vec.x = b.x * multNum;
            vec.y = b.y * multNum;
            vec.z = b.z * multNum;
            return vec;
        }

        public static Vector3 operator /(Vector3 b, double multNum) {
            if (multNum == 0) {
                multNum = 1;
            }
            Vector3 vec = new Vector3();
            vec.x = b.x / multNum;
            vec.y = b.y / multNum;
            vec.z = b.z / multNum;
            return vec;
        }
    }
    #endregion

    #region RAY 
    public class Ray {
        public Vector3 rayOrigin, rayDirection;
        public Ray(Vector3 rayOrigin, Vector3 rayDirection) {
            this.rayOrigin = rayOrigin;
            this.rayDirection = rayDirection;
        }
    }
    #endregion

    #region SPHERE
    public class Sphere {
        public double radius;
        public Vector3 sphereOrigin;
        public Color albedo;

        public Sphere(Vector3 sphereOrigin) {
            this.sphereOrigin = sphereOrigin;
            radius = 1;
        }

        public Sphere(Vector3 sphereOrigin, double radius) {
            this.sphereOrigin = sphereOrigin;
            this.radius = radius;
        }
    }
    #endregion

    #region SPHERE_LIGHT
    public class DirectionalLight {
        public Vector3 direction;
        public double intensity;
        public Color lightColor;

        public DirectionalLight() {
            this.direction = new Vector3(0, 0, -1);
            direction.Normalize();
            this.intensity = 20;
            this.lightColor = Color.LightYellow;
        }
        public DirectionalLight(Vector3 lightDirection) {
            this.direction = lightDirection;
            direction.Normalize();
            this.intensity = 20;
            this.lightColor = Color.LightYellow;
        }
    }
    #endregion
}
