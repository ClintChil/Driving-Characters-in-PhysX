using UnityEngine;
using System.Collections;

public class MathFL {
    // ***********************************   I USE THIS TO DO A LITTLE 2D MATH AND USE A COUPLE LOOKUP TABLES  ***********
	static public float timer = 0;
    static float[] sinLookupTable;
    static float[] cosLookupTable;
    static float[,] angleLookupTable;
    static int lookupResolution = 2000;
    static Vector3[] vector3LookupTable;
    static Vector2[] vector2LookupTable;
    static int pointIndex = 0;
    static Vector3[] pointOnSphereTable;
    static Vector3[] pointInsideSphereTable;
    static public float PI2 = 1;
    static public float invPI2 = 1;
    static bool setup = false;
    //
    static public void SetupLookupTables() {

        if (!setup) {
            PI2 = Mathf.PI * 2;
            invPI2 = 1 / PI2;
            sinLookupTable = new float[lookupResolution];
            for (int i = 0; i < lookupResolution; i++) {
                sinLookupTable[i] = Mathf.Sin(Mathf.PI * 2 / lookupResolution * i);
            }
            cosLookupTable = new float[lookupResolution];
            for (int i = 0; i < lookupResolution; i++) {
                cosLookupTable[i] = Mathf.Cos(Mathf.PI * 2 / lookupResolution * i);
            }
            vector3LookupTable = new Vector3[lookupResolution];
            for (int i = 0; i < lookupResolution; i++) {
                vector3LookupTable[i] = new Vector3(Cos(Mathf.PI * 2 / lookupResolution * i), 0, Sin(Mathf.PI * 2 / lookupResolution * i));

            }
            vector2LookupTable = new Vector2[lookupResolution];
            for (int i = 0; i < lookupResolution; i++) {
                vector2LookupTable[i] = new Vector2(Sin(Mathf.PI * 2 / lookupResolution * i), Cos(Mathf.PI * 2 / lookupResolution * i));
            }
            angleLookupTable = new float[lookupResolution, lookupResolution];
            for (int i = 0; i < lookupResolution; i++) {
                for (int j = 0; j < lookupResolution; j++) {
                    angleLookupTable[i, j] = Mathf.Atan2(j - lookupResolution / 2, i - lookupResolution / 2);
                    // vectorLookupTable[i] = new Vector3(Cos(Mathf.PI * 2 / lookupResolution * i), Sin(Mathf.PI * 2 / lookupResolution * i), 0);

                }
            }
            pointOnSphereTable = new Vector3[lookupResolution];
            for (int i = 0; i < lookupResolution; i++) {
                pointOnSphereTable[i] = Random.onUnitSphere;
            }
            pointInsideSphereTable = new Vector3[lookupResolution];
            for (int i = 0; i < lookupResolution; i++) {
                pointInsideSphereTable[i] = Random.insideUnitSphere;
            }
            setup = true;
        }
    }
    static public float Sin(float a) {
        
        return sinLookupTable[(int)(Mathf.Repeat(a, PI2) * invPI2 * lookupResolution)];
    }
    static public float Cos(float a) {
        return cosLookupTable[(int)(Mathf.Repeat(a, PI2) * invPI2 * lookupResolution)];
    }
    static public Vector3 RandomPointOnCircle() {
		return Point3OnCircle(Random.value * PI2 , 1f);			
	}
    static public Vector3 Point3OnCircle(float angle, float radius) {
      
        return vector3LookupTable[(int)(Mathf.Repeat(angle, PI2) * invPI2 * lookupResolution)]*radius;
      
    }
    static public Vector2 Point2OnCircle(float angle, float radius) {
        
        return vector2LookupTable[(int)(Mathf.Repeat(angle, PI2) * invPI2 * lookupResolution)] * radius;

    }

	//static public Vector2 Point2OnCircle(float angle, float radius) {
        
	//	Vector2 u = Vector2.zero;		
	//	float c  = Cos(angle);
	//	float s = Sin(angle);
	//	u.x =(s * radius);
	//	u.y =(c * radius);
	//	return u;
	//}
    //
    static public Vector3 GetRandomPointOnSphere() {
        pointIndex++;
        if (pointIndex >= 1000) pointIndex = 0;
        return pointOnSphereTable[pointIndex];
    }
    static public Vector3 GetRandomPointInsideSphere() {
        pointIndex++;
        if (pointIndex >= 1000) pointIndex = 0;
        return pointInsideSphereTable[pointIndex];
    }
    //
	static public Vector3 ForceZeroZ( Vector3 v) {
		return new Vector3(v.x , v.y, 0);
	}
	static public int RandomNegativePositive() {
		if (Random.value > 0.5f) {
			return -1;
		} else {
			return 1;
		}
	}
    
    static public float GetAngle(float x, float z) {
       // float newX = x / (Mathf.Abs(x) + Mathf.Abs(y));
       // y = (1 - newX) * Mathf.Sign(y);
       // x = newX* Mathf.Sign(x);
       // return angleLookupTable[(int)(x * lookupResolution/2), (int)(y * lookupResolution/2)];
        return Mathf.Atan2(z, x);
    }
   
	static public float GetAngle(Vector2 v) {		
		return Mathf.Atan2(v.y , v.x) ;		
	}
    static public float GetAngle(Vector3 v) {
        return Mathf.Atan2(v.z, v.x);
    }


   
    

}
