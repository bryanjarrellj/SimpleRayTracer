using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace RayTracer {
    class Program {
        //TODO: AA Implementation
        //TODO: Add intersection tests for planes, triangles.  
        //TODO: Add .obj or .blend mesh support.
        public static bool TestSphereIntersect(Ray rayTest, Sphere sphereTest, out double tValue) {
            tValue = 0;
            //Test intersection between ray and our sphere.
            Vector3 centerLength = new Vector3(sphereTest.sphereOrigin.x - rayTest.rayOrigin.x,
                                                sphereTest.sphereOrigin.y - rayTest.rayOrigin.y,
                                                sphereTest.sphereOrigin.z - rayTest.rayOrigin.z);

            //Simple Dot product check
            double tca = Vector3.Dot(centerLength, rayTest.rayDirection);

            if (tca < 0) {
                return false;
            }

            //We subtract tca to get the length of the last side of the triangle
            double dLength = Vector3.Dot(centerLength, centerLength) - tca * tca;
            if (dLength > sphereTest.radius) {
                return false;
            }

            //dLength can now be used to find the length inside the sphere, the last side of the smaller triangle
            //which can then in turn be used to find the hit point on the sphere
            double thc = Math.Sqrt(sphereTest.radius - dLength);
            double t0 = tca - thc;
            double t1 = tca + thc;

            if (t0 > t1) {
                double tmpSwp = t0;
                t0 = t1;
                t1 = tmpSwp;
            }

            if (t0 < 0) {
                if (t1 < 0) {
                    return false;
                } else {
                    t0 = t1;
                }
            }

            tValue = t0;

            return true;

        }

        static void Main(string[] args) {
            //TODO: Allow custom image pixel sizes.
            int screenX = 500;
            int screenY = 500;
            //Vector3 u = new Vector3(1, 0, 0);
            //Vector3 v = new Vector3(0, 1, 0);

            //Z axis point of the direction vector
            Vector3 w = new Vector3(0, 0, -1);
            Vector3 cameraPos = new Vector3(0);

            //Width and height of the image plane
            //Think of it like photosensitive paper
            float viewRectWidth = 1;
            float viewRectHeight = 1;

            Vector3 currentRayOrigin = new Vector3(0);

            List<Sphere> sphereList = new List<Sphere>();

            //Add in a new light
            DirectionalLight light1 = new DirectionalLight(new Vector3(-1, -1, -1));

            //TODO: Calculate random sphere position

            Sphere sphere1 = new Sphere(new Vector3(0, 0, -5), 1);
            sphere1.albedo = Color.FromArgb(255, (int)(0.18 * 255.0), 0, 0);
            Sphere sphere2 = new Sphere(new Vector3(2.0, 0.2, -8), 0.5);
            sphere2.albedo = Color.FromArgb(255, 0, 0, (int)(0.18 * 255.0));

            sphereList.Add(sphere1);
            sphereList.Add(sphere2);

            Ray rayTest = new Ray(cameraPos, w);

            double tmpU = 0, tmpV = 0;
            double left = 0 - (viewRectWidth / 2.0);
            double right = viewRectWidth - (viewRectWidth / 2.0);
            double top = viewRectHeight - (viewRectHeight / 2.0);
            double bottom = 0 - (viewRectHeight / 2.0);
            double pixelWidth = viewRectWidth / screenX;
            double pixelHeight = viewRectHeight / screenY;

            double tValue = 0;

            //Bitmap for use to create image later.
            Bitmap bitmap = new Bitmap(screenX, screenY);


            for (int y = 0; y < screenY; y++) {
                for (int x = 0; x < screenX; x++) {
                    tmpU = left + (pixelWidth / 2.0) + (x * pixelWidth);
                    tmpV = top - (pixelHeight / 2.0) - (y * pixelHeight);

                    //What about perspective view?
                    rayTest.rayDirection.x = tmpU;
                    rayTest.rayDirection.y = tmpV;
                    rayTest.rayDirection.z = w.z;
                    rayTest.rayOrigin.z = cameraPos.z;
                    //Normalize the ray direction
                    rayTest.rayDirection.Normalize();

                    double minDistance = double.MaxValue;
                    Sphere foundSphere = null;

                    for (int k = 0; k < sphereList.Count; k++) {
                        if (TestSphereIntersect(rayTest, sphereList[k], out tValue)) {
                            if (tValue < minDistance) {
                                minDistance = tValue;
                                foundSphere = sphereList[k];
                            }
                        }
                    }

                    if (foundSphere != null) {
                        Vector3 pHit = new Vector3(rayTest.rayOrigin.x, rayTest.rayOrigin.y, rayTest.rayOrigin.z);
                        pHit.x += minDistance * rayTest.rayDirection.x;
                        pHit.y += minDistance * rayTest.rayDirection.y;
                        pHit.z += minDistance * rayTest.rayDirection.z;

                        Vector3 nHit = new Vector3(pHit.x - foundSphere.sphereOrigin.x,
                                                   pHit.y - foundSphere.sphereOrigin.y,
                                                   pHit.z - foundSphere.sphereOrigin.z);

                        nHit.Normalize();

                        //Facing direction is the lighting facing the camera directly.
                        //Comment this out if using a light source instead
                        //Vector3 facingDirection = new Vector3(rayTest.rayDirection.x * -1, rayTest.rayDirection.y * -1, rayTest.rayDirection.z * -1);
                        //facingDirection.Normalize();

                        //int hitColor = (int)(Math.Max(0.0f, Vector3.Dot(nHit, facingDirection)) * 255);
                        //double dotFactor = Math.Max(0.0f, Vector3.Dot(nHit, facingDirection));

                        //Lighting for a light source instead!
                        double dotFactor = Math.Max(0.0f, Vector3.Dot(nHit, -light1.direction));

                        //Colors for facing direction
                        //int finalR = (int) (dotFactor * foundSphere.sphereColor.R);
                        //int finalG = (int) (dotFactor * foundSphere.sphereColor.G);
                        //int finalB = (int) (dotFactor * foundSphere.sphereColor.B);
                        //Colors for a real light.
                        double finalRatioR = ((((double)foundSphere.albedo.R / 255.0) / Math.PI) * light1.intensity * ((double)light1.lightColor.R / 255.0) * dotFactor);
                        double finalRatioG = ((((double)foundSphere.albedo.G / 255.0) / Math.PI) * light1.intensity * ((double)light1.lightColor.G / 255.0) * dotFactor);
                        double finalRatioB = ((((double)foundSphere.albedo.B / 255.0) / Math.PI) * light1.intensity * ((double)light1.lightColor.B / 255.0) * dotFactor);
                        int finalR = Math.Min(255, (int)(255.0 * finalRatioR));
                        int finalG = Math.Min(255, (int)(255.0 * finalRatioG));
                        int finalB = Math.Min(255, (int)(255.0 * finalRatioB));
                        Color finalColor = Color.FromArgb(255, finalR, finalG, finalB);

                        //Console.WriteLine(Math.Max(0.0f, Vector3.Dot(nHit, facingDirection)));

                        //Make color red for now.
                        //bitmap.SetPixel(x, y, Color.FromArgb(255, hitColor, 0, 0));
                        //Make final color sphere color of closest sphere
                        bitmap.SetPixel(x, y, finalColor);
                    } else {
                        //Set black for now.
                        //TODO: Allow cmd line bg color, or add bg color flag.
                        bitmap.SetPixel(x, y, Color.FromArgb(255, 100, 100, 100));
                    }
                }
            }

            Image i = (Image)bitmap;

            string outputName = args.Length > 0 && args[0] != null ? args[0].ToString() : "rayTraceDefault.png";

            if (System.IO.Directory.Exists("../../Output")) {
                i.Save("../../Output/" + outputName + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
            //Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            Console.WriteLine("Done running");
        }
    }
}
