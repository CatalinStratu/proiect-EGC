using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;

/**
    Aplicația utilizează biblioteca OpenTK v2.0.0 (stable) oficială și OpenTK. GLControl v2.0.0
    (unstable) neoficială. Aplicația fiind scrisă în modul consolă nu va utiliza controlul WinForms
    oferit de OpenTK!
    Tipul de ferestră utilizat: GAMEWINDOW. Se demmonstrează modul imediat de randare (vezi comentariu!)...
**/
namespace Piramide_console{
    class SimpleWindow3D : GameWindow {

        const float rotation_speed = 180.0f;
        double xrot, yrot, zrot = 0;
        bool showPiramide = true;
        KeyboardState lastKeyPress;
        private float q;
        private float f;

       
        // Constructor.
        public SimpleWindow3D() : base(900, 700) {
            VSync = VSyncMode.On;
            
        }

        // Setare mediu OpenGL și încarcarea resurselor (dacă e necesar) - de exemplu culoarea de
        // fundal a ferestrei 3D.
        // Atenție! Acest cod se execută înainte de desenarea efectivă a scenei 3D.
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            GL.ClearColor(Color.SteelBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthTest);

        }

        // Inițierea afișării și setarea viewport-ului grafic. Metoda este invocată la redimensionarea
        // ferestrei. Va fi invocată o dată și imediat după metoda ONLOAD!
        // Viewport-ul va fi dimensionat conform mărimii ferestrei active (cele 2 obiecte pot avea și mărimi 
        // diferite). 
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 5, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
            
        }

        // Secțiunea pentru "game logic"/"business logic". Tot ce se execută în această secțiune va fi randat
        // automat pe ecran în pasul următor - control utilizator, actualizarea poziției obiectelor, etc.
        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);
 
            //angle += rotation_speed * (float)e.Time;
            //GL.Rotate(angle, 0.0f, 1.0f, 0.0f);

            KeyboardState keyboard = OpenTK.Input.Keyboard.GetState();
            MouseState mouse = OpenTK.Input.Mouse.GetState();
            int x_click = mouse.X;
            int y_click = mouse.Y;


            if (keyboard[OpenTK.Input.Key.Escape]) {
                this.Exit();
                return;
            } else if (keyboard[OpenTK.Input.Key.P] && !keyboard.Equals(lastKeyPress)) {
                // Ascundere comandată, prin apăsarea unei taste - cu verificare de remanență! Timpul de reacție
                // uman << calculator.
                if (showPiramide == true) {
                    showPiramide = false;
                } else {
                    showPiramide = true;
                }
            }
            if (keyboard[OpenTK.Input.Key.X])
            {
                GL.Rotate(-1, 1, 1,1);
            }
            else if ((x_click!=X || y_click!=Y) && mouse[MouseButton.Left] )
            {
                GL.Viewport(x_click, -y_click, Width, Height);
            }

            if (keyboard[Key.Left]) 
            {
                q -= 0.05f;
            }
            if (keyboard[Key.Right])
            {
                q += 0.05f;
            }
            if (keyboard[Key.Down])
            {
                f -= 0.05f;
            }
            if (keyboard[Key.Up])
            {
                f += 0.05f;
            }
      
            lastKeyPress = keyboard;
        }

        // Secțiunea pentru randarea scenei 3D. Controlată de modulul logic din metoda ONUPDATEFRAME.
        // Parametrul de intrare "e" conține informatii de timing pentru randare.
        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);
          


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            // Exportăm controlul randării obiectelor către o metodă externă (modularizare).
            if (showPiramide == true) {
                DrawPiramide();
            }

     
            this.SwapBuffers();
            //Thread.Sleep(1);
        }

        // utilizăm modul imediat!!!
        private void DrawPiramide() {
         
                GL.Begin(PrimitiveType.Triangles);
                GL.LoadIdentity();                 // load the identity matrix

                GL.Color3(Color.GreenYellow);
                GL.Vertex3(-1.0f + q, -1.0f + f, -1.0f );
                GL.Vertex3(-1.0f + q, -1.0f + f, 1.0f );
                GL.Vertex3(1.0f + q, -1.0f + f, 1.0f );

                GL.Color3(Color.OrangeRed);
                GL.Vertex3(1.0f + q, -1.0f + f, 1.0f );
                GL.Vertex3(1.0f + q, -1.0f + f, -1.0f );
                GL.Vertex3(-1.0f + q, -1.0f + f, -1.0f );

                GL.Color3(Color.DarkGoldenrod);
                GL.Vertex3(-1.0f + q, -1.0f + f, -1.0f );
                GL.Vertex3(-0.05f + q, 1.05f + f, 0.0f );
                GL.Vertex3(1.0f + q, -1.0f + f, -1.0f );

                GL.Color3(Color.Green);
                GL.Vertex3(1.0f + q, -1.0f + f, -1.0f );
                GL.Vertex3(-0.05f + q, 1.05f + f, 0.0f );
                GL.Vertex3(1.0f + q, -1.0f + f, 1.0f );

                GL.Color3(Color.Aquamarine);
                GL.Vertex3(1.0f + q, -1.0f + f, 1.0f );
                GL.Vertex3(-0.05f + q, 1.05f + f, 0.0f );
                GL.Vertex3(-1.0f + q, -1.0f + f, 1.0f );

                GL.Color3(Color.Maroon);
                GL.Vertex3(-1.0f + q, -1.0f + f, 1.0f );
                GL.Vertex3(-0.05f + q, 1.05f + f, 0.0f );
                GL.Vertex3(-1.0f + q, -1.0f + f, -1.0f );

                GL.End();  
        }

        [STAThread]
        static void Main(string[] args) {

            // Utilizarea cuvântului-cheie "using" va permite dealocarea memoriei o dată ce obiectul nu mai este
            // în uz (vezi metoda "Dispose()").
            // Metoda "Run()" specifică cerința noastră de a avea 30 de evenimente de tip UpdateFrame per secundă
            // și un număr nelimitat de evenimente de tip randare 3D per secundă (maximul suportat de subsistemul
            // grafic). Asta nu înseamnă că vor primi garantat respectivele valori!!!
            // Ideal ar fi ca după fiecare UpdateFrame să avem si un RenderFrame astfel încât toate obiectele generate
            // în scena 3D să fie actualizate fără pierderi (desincronizări între logica aplicației și imaginea randată
            // în final pe ecran).
            using (SimpleWindow3D example = new SimpleWindow3D()) {

                // Verificați semnătura funcției în documentația inline oferită de IntelliSense!
                example.Run(30.0, 0.0);
            }
        }
    }
}
