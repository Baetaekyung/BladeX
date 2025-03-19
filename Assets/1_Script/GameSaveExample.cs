using Leguar.TotalJSON;
using Swift_Blade.SaveSystem;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace Swift_Blade
{
    public class GameSaveExample : MonoBehaviour
    {
        private void Awake()
        {
            //Debug.Log("start" + Environment.CurrentManagedThreadId);
            //int r = await TestFunction();

            //sync version

            //JSON obj = new JSON();
            //obj.Add("ming3", 1);
            //obj.Add("ming2", 1);

            //save
            //SaveManager.SaveFile(obj);

            //load
            //JSON r = SaveManager.LoadFile();
            //Debug.Log(r.GetInt("ming3"));

            //async version

            //save
            //JSON obj = new JSON();
            //obj.Add("ming3", 255);
            //obj.Add("ming2", 1);
            //await SaveManager.SaveFileAsync(obj);

            //load
            //Debug.Log("load start");
            //JSON result = await SaveManager.LoadFileAsync();
            //Debug.Log("load end");
            //Debug.Log("result" + result.GetInt("ming3"));

            //replace
            //Debug.Log("replace value");
            //await result.ReplaceValueAsync("ming3", 125);
            //Debug.Log("Start saving");
            //result.Save().Forget();
        }
        private async ValueTask<int> TestFunction()
        {
            Debug.Log(Environment.CurrentManagedThreadId);
            await Awaitable.BackgroundThreadAsync();
            Debug.Log("bg" + Environment.CurrentManagedThreadId);
            await Funct2();
            Debug.Log(Environment.CurrentManagedThreadId);
            await Funct2();
            Debug.Log(Environment.CurrentManagedThreadId);
            //Funct2();

            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();

            //Debug.Log("normalDelay");
            //while (stopwatch.ElapsedMilliseconds < 3000)
            //{
            //}
            //stopwatch.Stop();
            Debug.Log("threadSleep");
            Thread.Sleep(2000);
            Debug.Log(Environment.CurrentManagedThreadId);
            //await Task.Delay(3000);

            Debug.Log("end");
            return 1;
        }
        private async ValueTask Funct2()
        {
            await Task.Delay(100);
            Debug.Log("Funct2_" + Environment.CurrentManagedThreadId);
        }
    }
}
