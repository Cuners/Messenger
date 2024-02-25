using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MessAnotherMaket.Models;
namespace MessAnotherMaket
{
    public class LoginViewModel : ViewModel
    {
        private string _login;
        public string Login
        {
            get { return _login; }
            set => Set(ref _login, value);
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set => Set(ref _password, value);
            
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set => Set(ref _email, value);

        }
        private RelayCommands _AuthCommand;

        public RelayCommands AuthCommand
        {
            get
            {
                return _AuthCommand ?? (_AuthCommand = new RelayCommands(async obj =>
                {
                    using (MessengEntities messengEntities = new MessengEntities())
                    {
                        var passbox = obj as PasswordBox;
                        var password = passbox.Password;
                        if (string.IsNullOrWhiteSpace(Login))
                        {
                            MessageBox.Show("Вы не ввели логин");
                            return;
                        }
                        await Task.Run(() =>
                        {
                            foreach (var polzovatel in messengEntities.Users)
                            {
                                if (polzovatel.Login == Login && polzovatel.Password == password)
                                {
                                    LoginMod.IdUserNow = polzovatel.Id;
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        MainWindow mainWindow = new MainWindow();
                                        mainWindow.Show();
                                    });
                                    return; 
                                }
                            }
                        });
                        //await Task.Run(() =>  /* Если данных много*/
                        //{
                        //    Parallel.ForEach(messengEntities.Users, polzovatel =>
                        //    {
                        //        if (polzovatel.Login == Login && polzovatel.Password == password)
                        //        {
                        //            LoginMod.IdUserNow = polzovatel.Id;
                        //            Application.Current.Dispatcher.Invoke(() =>
                        //            {
                        //                MainWindow mainWindow = new MainWindow();
                        //                mainWindow.Show();
                        //            });
                        //        }
                        //    });
                        //});

                    }
                }));
            }
        }
    }
}
