using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsPhoneLibrary
{
    public class FileIO
    {
        public void SaveTextToFile(string filename, string text)
        {
            using (Mutex mutex = new Mutex(true, "MyData"))
            {
                mutex.WaitOne();
                try
                {
                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (IsolatedStorageFileStream rawStream = isf.CreateFile(filename))
                        {
                            StreamWriter writer = new StreamWriter(rawStream);
                            writer.Write(text);
                            writer.Close();
                        }
                    }
                }
                catch { }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        public bool LoadTextFromFile(string filename, out string result)
        {
            result = "";
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(filename))
                {
                    using (Mutex mutex = new Mutex(true, "MyData"))
                    {
                        mutex.WaitOne();
                        try
                        {
                            using (IsolatedStorageFileStream rawStream = isf.OpenFile(filename, System.IO.FileMode.Open))
                            {
                                StreamReader reader = new StreamReader(rawStream);
                                result = reader.ReadToEnd();
                                reader.Close();
                            }
                        }
                        catch
                        {
                            return false;
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void SaveTextToSettings(string filename, string text)
        {
            using (Mutex mutex = new Mutex(true, "MyData"))
            {
                mutex.WaitOne();
                try
                {
                    IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;
                    isolatedStore.Remove(filename);
                    isolatedStore.Add(filename, text);
                    isolatedStore.Save();
                }
                catch { }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        public bool LoadTextFromSettings(string filename, out string result)
        {
            IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            result = "";
            using (Mutex mutex = new Mutex(true, "MyData"))
            {
                mutex.WaitOne();
                try
                {
                    result = (string)isolatedStore[filename];
                }
                catch
                {
                    return false;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            return true;
        }
    }
}
