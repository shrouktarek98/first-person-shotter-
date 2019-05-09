using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.IO;
using Graphics._3D_Models;
namespace Graphics
{

    class Renderer
    {
        //old variables
        Shader sh;
        int transID;
        int viewID;
        int projID;
        int EyePositionID;
        int AmbientLightID;
        int DataID;
        mat4 ProjectionMatrix;
        mat4 ViewMatrix;


        public float Speed = 3;
        uint vertexBufferID;
        uint vertexBufferID2;

        public Camera cam;


        vec3 playerPos;
        Model3D m;
        uint ShootID;
        Texture shoot;
        int count = 0;
        public md2LOL m2;

        mat4 left;
        mat4 right;
        mat4 back;
        mat4 front;
        mat4 top;
        mat4 grdd;
        public md2 blade;
        public md2 samourai;
        public md2 drfreak;
        Texture tex1;
        Texture grd;
        Texture topp;
        Texture sider;
        Texture sidel;
        Texture sidef;
        Texture sideb;
        mat4 modelmatrix;
        Model3D car;
        Model3D car2;
        Model3D house;
        Model3D tree;
        Model3D building;
        Model3D guardpost;
        Model3D spider;
        Model3D bull;
        vec3 drpos = new vec3(-200, 20, 90);
        vec3 m2pos = new vec3(-70, 0, 10);
        vec3 bladepos = new vec3(0, 38, 250);
        vec3 samupos = new vec3(0, 33, -50);
       
        List<bullet> bullets = new List<bullet>();

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            tex1 = new Texture(projectPath + "\\Textures\\base.jpg", 2);
            grd = new Texture(projectPath + "\\Textures\\sand.jpg", 3);
            topp = new Texture(projectPath + "\\Textures\\top.jpg", 4);
            sider = new Texture(projectPath + "\\Textures\\right.jpg", 5);
            sidel = new Texture(projectPath + "\\Textures\\left.jpg", 6);
            sidef = new Texture(projectPath + "\\Textures\\front.jpg", 7);
            sideb = new Texture(projectPath + "\\Textures\\back.jpg", 8);
            shoot = new Texture(projectPath + "\\Textures\\gunshot.png", 9);



            drfreak = new md2(projectPath + "\\ModelFiles\\animated\\md2\\drfreak\\drfreak.md2");

            drfreak.rotationMatrix = glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0));
            drfreak.scaleMatrix = glm.scale(new mat4(1), new vec3(1.2f, 1.2f, 1.2f));
            drfreak.TranslationMatrix = glm.translate(new mat4(1), drpos);


            m2 = new md2LOL(projectPath + "\\ModelFiles\\zombie.md2");
            m2.StartAnimation(animType_LOL.STAND);
            m2.rotationMatrix = glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0));
            m2.scaleMatrix = glm.scale(new mat4(1), new vec3(0.8f, 0.8f, 0.8f));
            m2.TranslationMatrix = glm.translate(new mat4(1), m2pos);


            blade = new md2(projectPath + "\\ModelFiles\\animated\\md2\\blade\\Blade.md2");


            blade.rotationMatrix = MathHelper.MultiplyMatrices(new List<mat4>()
                    {

                        glm.rotate((float)((270f/180)*Math.PI),new vec3(1,0,0)),glm.rotate((float)((1f/180)*Math.PI),new vec3(0,0,1))
                    });

            blade.scaleMatrix = glm.scale(new mat4(1), new vec3(0.9f, 0.9f, 0.9f));
            blade.TranslationMatrix = glm.translate(new mat4(1), bladepos);

            samourai = new md2(projectPath + "\\ModelFiles\\animated\\md2\\samourai\\Samourai.md2");
            samourai.StartAnimation(animType.STAND);
            samourai.rotationMatrix = glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0));
            samourai.scaleMatrix = glm.scale(new mat4(1), new vec3(0.5f, 0.5f, 0.5f));
            samourai.TranslationMatrix = glm.translate(new mat4(1), samupos);



            building = new Model3D();
            building.LoadFile(projectPath + "\\ModelFiles\\obj files\\building", 10, "Building 02.obj");
            building.scalematrix = glm.scale(new mat4(1), new vec3(15, 15, 15));
            building.transmatrix = glm.translate(new mat4(1), new vec3(0, 19.655f, -50));


            car = new Model3D();
            car.LoadFile(projectPath + "\\ModelFiles\\obj files\\car", 11, "dpv.obj");
            car.scalematrix = glm.scale(new mat4(1), new vec3(0.10f, 0.10f, 0.10f));
            car.transmatrix = glm.translate(new mat4(1), new vec3(70, 1, -50));
            car.rotmatrix = glm.rotate(3.1412f, new vec3(0, 1, 0));



            car2 = new Model3D();
            car2.LoadFile(projectPath + "\\ModelFiles\\obj files\\jeep", 12, "jeep1.3ds");
            car2.scalematrix = glm.scale(new mat4(1), new vec3(7, 7, 7));
            car2.transmatrix = glm.translate(new mat4(1), new vec3(90, 0, 250));
            car2.rotmatrix = glm.rotate(-90.0f / 180.0f * 3.1412f, new vec3(1, 0, 0));



            house = new Model3D();
            house.LoadFile(projectPath + "\\ModelFiles\\obj files\\House", 13, "house.3ds");
            house.scalematrix = glm.scale(new mat4(1), new vec3(20, 20, 20));
            house.rotmatrix = glm.rotate(90.0f / 180.0f * 3.1412f, new vec3(0, 1, 0));
            house.transmatrix = glm.translate(new mat4(1), new vec3(-200, 5, 120));


            guardpost = new Model3D();
            guardpost.LoadFile(projectPath + "\\ModelFiles\\obj files\\Guard_post", 14, "guard post.obj");
            guardpost.scalematrix = glm.scale(new mat4(1), new vec3(30, 30, 30));
            guardpost.rotmatrix = glm.rotate(3.1412f, new vec3(0, 1, 0));
            guardpost.transmatrix = glm.translate(new mat4(1), new vec3(0, 16, 250));



            tree = new Model3D();
            tree.LoadFile(projectPath + "\\ModelFiles\\obj files\\tree", 15, "Tree.obj");
            tree.scalematrix = glm.scale(new mat4(1), new vec3(10f, 10f, 10f));
            tree.transmatrix = glm.translate(new mat4(1), new vec3(-200, 0, 250));


            spider = new Model3D();
            spider.LoadFile(projectPath + "\\ModelFiles\\obj files\\spider", 16, "spider.obj");
            spider.scalematrix = glm.scale(new mat4(1), new vec3(0.1f, 0.1f, 0.1f));
            spider.rotmatrix = glm.rotate(90.0f / 180.0f * 3.1412f, new vec3(0, 1, 0));
            spider.transmatrix = glm.translate(new mat4(1), new vec3(0, 3.85f, -5));



            bull = new Model3D();
            bull.LoadFile(projectPath + "\\ModelFiles//Textured Bullet OBJ", 17, "lowpolybullet.obj");
            bull.scalematrix = glm.scale(new mat4(1), new vec3(0.5f, 0.5f, 0.5f));


            float[] shootvertices = {
                -1,1,0,
                1,0,0,
                0,0,
                0,1,0,
                1,-1,0,
                0,0,1,
                1,1,
                0,1,0,
                -1,-1,0,
                0,0,1,
                0,1,
                0,1,0,
                1,1,0,
                0,0,1,
                1,0,
                0,1,0,
                -1,1,0,
                0,1,0,
                0,0,
                0,1,0,
                1,-1,0,
                1,0,0,
                1,1,
                0,1,0
            };
            ShootID = GPU.GenerateBuffer(shootvertices);




            Gl.glClearColor(0, 0, 0.4f, 1);

            //camera
            cam = new Camera();
            cam.Reset(0, 0, 0, -15, 55, -1217, 0, 1, 0);
            cam.mAngleX = 2.8f;
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();


            m = new Model3D();
            m.LoadFile(projectPath + "\\ModelFiles\\hands with gun", 20, "gun.obj");

            playerPos = cam.GetCameraTarget();
            playerPos.y += 7;

            defaultY = cam.GetCameraTarget().y;

            m.scalematrix = glm.scale(new mat4(1), new vec3(0.2f, 0.2f, 0.2f));
            // m.rotmatrix = glm.rotate(3.1412f, new vec3(0, 1, 0));
            m.transmatrix = glm.translate(new mat4(1), playerPos);




            float[] ground2 = {
                -5.0f, 0.0f, 5.0f,
                 0,0,1,
                 0,1,

                 5.0f, 0.0f, -5.0f,
                 0,0,1,
                 1,0,

                 -5.0f, 0.0f, -5.0f,
                 0,0,1,
                 0,0,

                 5.0f, 0.0f, 5.0f,
                 0,0,1,
                 1,1,

                -5.0f, 0.0f, 5.0f,
                 0,0,1,
                 0,1,

                 5.0f, 0.0f, -5.0f,
                 0,0,1,
                 1,0,
            };
            vertexBufferID2 = GPU.GenerateBuffer(ground2);



            float[] ground = {
                -5.0f, 0.0f, 5.0f,
                 0,0,1,
                 0,1,

                 5.0f, 0.0f, -5.0f,
                 0,0,1,
                 1,0,

                 -5.0f, 0.0f, -5.0f,
                 0,0,1,
                 0,0,

                 5.0f, 0.0f, 5.0f,
                 0,0,1,
                 1,1,

                -5.0f, 0.0f, 5.0f,
                 0,0,1,
                 0,1,

                 5.0f, 0.0f, -5.0f,
                 0,0,1,
                 1,0,
            };
            vertexBufferID = GPU.GenerateBuffer(ground);




            Gl.glClearColor(0, 0, 0.4f, 1);

            cam = new Camera();
            cam.Reset(0, 34, 100, 0, 0, 0, 0, 1, 0);

            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            transID = Gl.glGetUniformLocation(sh.ID, "trans");
            projID = Gl.glGetUniformLocation(sh.ID, "projection");
            viewID = Gl.glGetUniformLocation(sh.ID, "view");

            grdd = MathHelper.MultiplyMatrices(new List<mat4>(){
                  glm.translate(new mat4(1),new vec3(0,0.005f,0)),
                  glm.scale(new mat4(1), new vec3(100, 100, 100))
             });

            right = MathHelper.MultiplyMatrices(new List<mat4>(){
               glm.rotate(90.0f / 180.0f * 3.1412f, new vec3(0, 0, 1)),
               glm.rotate(90.0f / 180.0f * 3.1412f, new vec3(1, 0, 0)),
               glm.translate(new mat4(1),new vec3(4.95f,4.95f,0)),
               glm.scale(new mat4(1), new vec3(100, 100, 100))

            });
            left = MathHelper.MultiplyMatrices(new List<mat4>(){
               glm.rotate(90.0f / 180.0f * 3.1412f, new vec3(0, 0, 1)),
                glm.rotate(90.0f / 180.0f * 3.1412f, new vec3(1, 0, 0)),
               glm.translate(new mat4(1),new vec3(-4.9f,4.95f,0)),
             glm.scale(new mat4(1), new vec3(100, 100, 100))
            });

            front = MathHelper.MultiplyMatrices(new List<mat4>(){
               glm.rotate(90.0f / 180.0f * 3.1412f, new vec3(1, 0, 0)),

               glm.translate(new mat4(1),new vec3(0,4.95f,4.95f)),
              glm.scale(new mat4(1), new vec3(100, 100, 100))
            });

            back = MathHelper.MultiplyMatrices(new List<mat4>(){
               glm.rotate(90.0f / 180.0f * 3.1412f, new vec3(1, 0, 0)),

               glm.translate(new mat4(1),new vec3(0,4.95f,-4.95f)),
               glm.scale(new mat4(1), new vec3(100, 100, 100))
            });

            top = MathHelper.MultiplyMatrices(new List<mat4>(){
               glm.translate(new mat4(1),new vec3(0,10f,0)),
               glm.scale(new mat4(1), new vec3(100, 100, 100))
            });
            modelmatrix = glm.scale(new mat4(1), new vec3(100, 100, 100));







            //shader configurations
            transID = Gl.glGetUniformLocation(sh.ID, "trans");
            projID = Gl.glGetUniformLocation(sh.ID, "projection");
            viewID = Gl.glGetUniformLocation(sh.ID, "view");
            sh.UseShader();

            //lights
            DataID = Gl.glGetUniformLocation(sh.ID, "data");
            vec2 data = new vec2(500, 50);
            Gl.glUniform2fv(DataID, 1, data.to_array());

            int LightPositionID = Gl.glGetUniformLocation(sh.ID, "LightPosition_worldspace");
            vec3 lightPosition = new vec3(1.0f, 500f, 4.0f);
            Gl.glUniform3fv(LightPositionID, 1, lightPosition.to_array());

            AmbientLightID = Gl.glGetUniformLocation(sh.ID, "ambientLight");
            vec3 ambientLight = new vec3(1, 1, 1);
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());

            EyePositionID = Gl.glGetUniformLocation(sh.ID, "EyePosition_worldspace");

            //enabling Depth test
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LESS);
            c = timer;
        }
        float defaultY;

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            //use 3D shader
            sh.UseShader();
            //draw 3D model



            playerPos = cam.GetCameraTarget();
            playerPos.y -= 2.5f;
            m.rotmatrix = glm.rotate(3.1412f + cam.mAngleX, new vec3(0, 1, 0));//* glm.rotate(cam.mAngleY, new vec3(1, 0, 0));
            m.transmatrix = glm.translate(new mat4(1), playerPos);
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            //send shader values
            Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniform3fv(EyePositionID, 1, cam.GetCameraPosition().to_array());


            m.Draw(transID);
           m2.Draw(transID);
            drfreak.Draw(transID);
            blade.Draw(transID);
            building.Draw(transID);
            car.Draw(transID);
            car2.Draw(transID);
            house.Draw(transID);
            guardpost.Draw(transID);
            tree.Draw(transID);
            spider.Draw(transID);
            samourai.Draw(transID);

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, ShootID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)0);

            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            Gl.glEnableVertexAttribArray(3);
            Gl.glVertexAttribPointer(3, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(8 * sizeof(float)));

            shoot.Bind();
            vec3 shootpos = cam.GetCameraTarget();
            shootpos.y -= 1.5f;
            shootpos += cam.GetLookDirection() * 8;

            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, MathHelper.MultiplyMatrices(new List<mat4>() { glm.scale(new mat4(1),new vec3(2+(float)c/10,2 + (float)c / 10, 2 + (float)c / 10)),
                glm.rotate(cam.mAngleX, new vec3(0, 1, 0)),glm.rotate((float)c/10, new vec3(0, 0, 1)),glm.translate(new mat4(1),shootpos),
            }).to_array());
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            if (draw)
            {
                cam.mAngleY -= 0.01f;
                Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
                bull.transmatrix = glm.translate(new mat4(1), shootpos);
                bullet b = new bullet(shootpos, cam.GetLookDirection() * 8);
                b.draw(bull, transID);
                b.update();
                bullets.Add(b);
                c--;
                if (c < 0)
                {
                    cam.mAngleY = 0;
                    c = timer;
                    draw = false;
                }
            }
            Gl.glDisable(Gl.GL_BLEND);


            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertexBufferID);
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, modelmatrix.to_array());
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));
            //enable another vertex attribute for normals
            //describe the attribute and recompute the stride for all attributes

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertexBufferID2);
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, modelmatrix.to_array());
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            //Gl.glEnableVertexAttribArray(1);
            //  Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            grd.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, grdd.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

            tex1.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            sider.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, right.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            sidel.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, left.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            sideb.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, front.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            sidef.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, back.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            topp.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, top.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);


        }
        int timer = 5;
        int c;
        public bool draw = false;
        vec2 r = new vec2();
        vec3 dis;
        public float range;
        public float angle;
        public void Update(float deltaTime)
        {
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            //zombie
            
            r.x = cam.mCenter.x - m2pos.x;
            r.y = cam.mCenter.z - m2pos.z;
            range = (float)(Math.Sqrt(r.x * r.x + r.y * r.y));
            if (range > 10 && range < 80)
            {
                
                m2pos = m2.move(m2pos, r);
                dis = new vec3();
                dis = playerPos - m2pos;
                dis = glm.normalize(dis);
                angle = m2.angle(dis);
                 m2.rotationMatrix = MathHelper.MultiplyMatrices(new List<mat4>()
                    {
                glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0)),
                 glm.rotate((float)((-angle) * Math.PI), new vec3(0, 1, 0))
            });
            }
            m2.attackorrun(m2, range);
            m2.TranslationMatrix = glm.translate(new mat4(1), m2pos);
            m2.UpdateExportedAnimation();

            //drfreak
            
            r = new vec2();
            r.x = cam.mCenter.x - drpos.x;
            r.y = cam.mCenter.z - drpos.z;
            range = (float)(Math.Sqrt(r.x * r.x + r.y * r.y));
            if (range > 10 && range < 80)
            {
              
                drpos = drfreak.move(drpos, r);
                dis = new vec3();
                dis = playerPos - drpos;
                dis = glm.normalize(dis);
                angle = drfreak.angle(dis);
                drfreak.rotationMatrix = MathHelper.MultiplyMatrices(new List<mat4>()
                    {
                 glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0)),
                  glm.rotate((float)((angle)*Math.PI),new vec3(0,1,0))
                    });
            }
            drfreak.run(drfreak, range);
            drfreak.attack(drfreak, range);
            drfreak.stop(drfreak, range);
            drfreak.TranslationMatrix = glm.translate(new mat4(1), drpos);
            drfreak.UpdateExportedAnimation();

            //blade
           
            r = new vec2();
            r.x = cam.mCenter.x - bladepos.x;
            r.y = cam.mCenter.z - bladepos.z;
            range = (float)(Math.Sqrt(r.x * r.x + r.y * r.y));
           if (range > 10 && range < 100)
            {
                
                bladepos = blade.move(bladepos, r);
                dis = new vec3();
                dis = playerPos - bladepos;
                dis = glm.normalize(dis);
                angle = blade.angle(dis);
                blade.rotationMatrix = MathHelper.MultiplyMatrices(new List<mat4>()
                    {

                        glm.rotate((float)((270f/180)*Math.PI),new vec3(1,0,0)),
                        glm.rotate((float)((1f/180)*Math.PI),new vec3(0,0,1)),
                        glm.rotate((float)((-angle)*Math.PI),new vec3(0,1,0))
                    });
              

            }
            blade.run(blade, range);
            blade.stop(blade, range);
            blade.attack(blade, range);
            blade.TranslationMatrix = glm.translate(new mat4(1), bladepos);
            blade.UpdateExportedAnimation();
    

            //samurai
            r = new vec2();
            r.x = cam.mCenter.x - samupos.x;
            r.y = cam.mCenter.z - samupos.z;
            range = (float)(Math.Sqrt(r.x * r.x + r.y * r.y));
            /*if (range > 10 && range < 50)
            {
                samupos = samourai.move(samupos, r);
            }*/
            samourai.attack(samourai, range);
            samourai.stop(samourai, range);
            samourai.TranslationMatrix = glm.translate(new mat4(1), samupos);
            samourai.UpdateExportedAnimation();
        }
        
        public void SendLightData(float red, float green, float blue, float attenuation, float specularExponent)
        {
            vec3 ambientLight = new vec3(red, green, blue);
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());
            vec2 data = new vec2(attenuation, specularExponent);
            Gl.glUniform2fv(DataID, 1, data.to_array());
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
