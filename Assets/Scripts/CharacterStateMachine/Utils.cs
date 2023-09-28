using System;
using UnityEngine;

public class Utils
{

    public const double TO_RADIAN = Math.PI / 180.0;

    // here we apply the rotate matrix to rotate vector on (x, z) plane
    // the matrix is 
    // cos(a), -sin(a)
    // sin(a), cos(a)
    // so the direction forward(x, y, z) will be 
    // cos(a) * x - sin(a) * z, y, sin(a) * x + cos(a) * z
    public static Vector3 ROTATE_X_Z(float degree, Vector3 toRotate)
    {
        double cos = Math.Cos(degree * TO_RADIAN);
        double sin = Math.Sin(degree * TO_RADIAN);
        double sinm = -1*Math.Sin(degree * TO_RADIAN);
        double x = cos * toRotate.x + sinm * toRotate.z;
        double z = sin * toRotate.x + cos * toRotate.z;
        Vector3 ret =  new Vector3();
        ret.x = (float)x;
        ret.y = toRotate.y;
        ret.z = (float)z;
        return ret;
    }

    // rotate x z 90 degree
    public static Vector3 ROTATE_X_Z_90D(Vector3 toRotate)
    {
        float cos = 0;
        float sin = 1;
        float sinm = -1;
        float x = cos * toRotate.x + sinm * toRotate.z;
        float z = sin * toRotate.x + cos * toRotate.z;
        Vector3 ret = new Vector3();
        ret.x = x;
        ret.y = toRotate.y;
        ret.z = z;
        return ret;
    }

    // rotate x z -90 degree
    public static Vector3 ROTATE_X_Z_M90D(Vector3 toRotate)
    {
        float cos = 0;
        float sin = -1;
        float sinm = 1;
        float x = cos * toRotate.x + sinm * toRotate.z;
        float z = sin * toRotate.x + cos * toRotate.z;
        Vector3 ret = new Vector3();
        ret.x = x;
        ret.y = toRotate.y;
        ret.z = z;
        return ret;
    }

    // rotate x z 180 degree
    public static Vector3 ROTATE_X_Z_180D(Vector3 toRotate)
    {
        float cos = -1;
        float sin = 0;
        float sinm = 0;
        float x = cos * toRotate.x + sinm * toRotate.z;
        float z = sin * toRotate.x + cos * toRotate.z;
        Vector3 ret = new Vector3();
        ret.x = x;
        ret.y = toRotate.y;
        ret.z = z;
        return ret;
    }

    public static float Distance(Vector3 a, Vector3 b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        float dz = a.z - b.z;
        float d2 = dx * dx + dy * dy + dz * dz;
        float dis = (float)Math.Pow((float)d2, 0.5d);
        return dis;
    }
}
