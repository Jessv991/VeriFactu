using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace VeriFactu
{
    internal class pathServer
    {

        public static string getPathServer()
        {
            string path_server_img = "";
            if (IntPtr.Size == 8)
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Wow6432Node\\VIVvision]"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("Servidor");
                        if (o != null)
                        {
                            path_server_img = o.ToString();
                        }
                    }
                    else
                    {
                        using (RegistryKey key_new = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\NewNog"))
                            if (key_new != null)
                            {
                                Object o = key_new.GetValue("Servidor");
                                if (o != null)
                                {
                                    path_server_img = o.ToString();
                                }
                            }
                    }
                }
            }
            else
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\VIVvision]"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("Servidor");
                        if (o != null)
                        {
                            path_server_img = o.ToString();
                        }
                    }
                    else
                    {
                        using (RegistryKey key_new = Registry.LocalMachine.OpenSubKey("Software\\NewNog"))
                            if (key_new != null)
                            {
                                Object o = key_new.GetValue("Servidor");
                                if (o != null)
                                {
                                    path_server_img = o.ToString();
                                }
                            }
                    }
                }
            }
            return path_server_img;
        }

        public static string getPathServerImg()
        {
            string path_server_img = "";
            if (IntPtr.Size == 8)
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Wow6432Node\\VIVvision]"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("Imagenes");
                        if (o != null)
                        {
                            path_server_img = o.ToString();
                        }
                    }
                    else
                    {
                        using (RegistryKey key_new = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\NewNog"))
                            if (key_new != null)
                            {
                                Object o = key_new.GetValue("Imagenes");
                                if (o != null)
                                {
                                    path_server_img = o.ToString();
                                }
                            }
                    }
                }
            }
            else
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\VIVvision]"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("Imagenes");
                        if (o != null)
                        {
                            path_server_img = o.ToString();
                        }
                    }
                    else
                    {
                        using (RegistryKey key_new = Registry.LocalMachine.OpenSubKey("Software\\NewNog"))
                            if (key_new != null)
                            {
                                Object o = key_new.GetValue("Imagenes");
                                if (o != null)
                                {
                                    path_server_img = o.ToString();
                                }
                            }
                    }
                }
            }
            return path_server_img;
        }


    }
}
