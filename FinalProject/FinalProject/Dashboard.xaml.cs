using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FinalProject
{	
	public partial class Dashboard : ContentPage
	{	
		public Dashboard ()
		{
			InitializeComponent ();

		}

        private async void OnBSIT1_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StudentList1());
        }

        private async void OnBSIT2_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StudentList2());
        }

        private async void OnBSIT3_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StudentList3());
        }

        private async void OnBSIT4_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StudentList4());
        }
    }
}

