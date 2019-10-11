using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WindesHeart.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted },
        Categories = new[] { Android.Content.Intent.CategoryDefault }
    )]
    public class ReceiveBoot: BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action != null && intent.Action == Intent.ActionBootCompleted)
            {
                Intent startServiceIntent = new Intent(context, typeof(ForegroundService));
                startServiceIntent.SetAction(Constants.ACTION_START_SERVICE);
                context.ApplicationContext.StartForegroundService(startServiceIntent);
            }
        }
    }
}