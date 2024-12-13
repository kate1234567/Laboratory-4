using FamilyTree.BLL;
using FamilyTree.DAL;
using FamilyTree.Presentation;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FamilyTree.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Human> Humans { get; set; } = new ObservableCollection<Human>();
        public ObservableCollection<FamilyTree.DAL.FamilyTree> FamilyTrees { get; set; } = new ObservableCollection<FamilyTree.DAL.FamilyTree>();
        public ObservableCollection<string> Relations { get; set; } = new ObservableCollection<string>()
        {
            "Родители","Ребенок"
        };
        public Relation Relation { get; set; } = new Relation();
        public Paint Paint { get; set; } = new Paint();

        private DBHandler _dbHandler = null;

        public MainWindowViewModel()
        {
            _dbHandler = new DBHandler(App.Configuration.GetConnectionString("DataBase"));

            clearHuman();
            setHumans();
            getTrees();
        }

        #region Props
        private string fio;
        public string FIO
        {
            get { return fio; }
            set
            {
                fio = value;
                OnPropertyChanged();
            }
        }

        private DateTime birthDay;
        public DateTime BirthDay
        {
            get { return birthDay; }
            set
            {
                birthDay = value;
                OnPropertyChanged();
            }
        }

        private bool genderMale;
        public bool GenderMale
        {
            get { return genderMale; }
            set
            {
                genderMale = value;
                OnPropertyChanged();
            }
        }

        private bool genderFemale;
        public bool GenderFemale
        {
            get { return genderFemale; }
            set
            {
                genderFemale = value;
                OnPropertyChanged();
            }
        }

        private string titleTree;
        public string TitleTree
        {
            get { return titleTree; }
            set
            {
                titleTree = value;
                OnPropertyChanged();
            }
        }

        private FamilyTree.DAL.FamilyTree selectedTree;
        public FamilyTree.DAL.FamilyTree SelectedTree
        {
            get { return selectedTree; }
            set
            {
                selectedTree = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods
        private void clearHuman()
        {
            FIO = "";
            BirthDay = DateTime.Now;
            GenderMale = false;
            GenderFemale = false;
        }

        private void setHumans()
        {
            Humans.Clear();
            var humans = _dbHandler.GetHumans();
            foreach (var human in humans)
            {
                Humans.Add(human);
            }
        }

        private void getTrees()
        {
            FamilyTrees.Clear();
            var trees = _dbHandler.GetFamilyTrees();
            foreach (var tree in trees)
            {
                FamilyTrees.Add(tree);
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddHuman
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (SelectedTree != null)
                    {
                        var gender = GenderMale == true ? DAL.Human.GenderEnum.Male : DAL.Human.GenderEnum.Female;
                        var human = new Human(FIO, BirthDay, gender, SelectedTree.Id);
                        _dbHandler.CreateOrUpdateHuman(human);
                        clearHuman();
                        setHumans();
                    }
                    else
                    {
                        MessageBox.Show("Не выбрано дерево", "Ошибка");
                    }
                });
            }
        }

        public RelayCommand AddTree
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    var tree = new FamilyTree.DAL.FamilyTree(TitleTree);
                    _dbHandler.CreateTree(tree);
                });
            }
        }

        public RelayCommand AddRelation
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    FamilyTree.BLL.Relation.SetRelation(_dbHandler, Relation);
                });
            }
        }

        public RelayCommand PaintTree
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (Paint.Tree != null)
                    {
                        var paint = new Paint(_dbHandler, Paint.Tree);
                        paint.PaintTreeBtn();
                    }
                    else
                    {
                        MessageBox.Show("Не выбрано дерево", "Ошибка");
                    }
                });
            }
        }

        public RelayCommand PaintTreeTxt
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    var paint = new Paint(_dbHandler, Paint.Tree);
                    paint.PaintTxt();
                });
            }
        }

        public RelayCommand FindRelation
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (Relation.HumanForRelation != null)
                    {
                        if (Relation.HumanForRelation.Childrens.Any() || Relation.HumanForRelation.Parents.Any())
                        {
                            var message = Relation.GetRelation();
                            MessageBox.Show(message, "Ближайшие родственники");
                        }
                        else
                        {
                            MessageBox.Show("Родственники не найдены", "Ошибка");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не выбран человек", "Ошибка");
                    }
                });
            }
        }

        public RelayCommand GetAge
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (Relation.HumanForGetAge != null)
                    {
                        if (Relation.HumanForGetAge.Childrens.Any())
                        {
                            MessageBox.Show(Relation.GetAge(), "Результат");
                        }
                        else
                        {
                            MessageBox.Show("У выбранного человека нет детей", "Ошибка");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не выбран человек", "Ошибка");
                    }
                });
            }
        }

        public RelayCommand GetCommon
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (Relation.Common.C1 != null && Relation.Common.C2 != null)
                    {
                        MessageBox.Show(Relation.GetCommon(), "Результат");
                    }
                    else
                    {
                        MessageBox.Show("Не все данные заполнены", "Ошибка");
                    }
                });
            }
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
