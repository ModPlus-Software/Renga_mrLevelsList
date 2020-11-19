namespace mrLevelsList.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Models;
    using ModPlusAPI.Mvvm;
    using ModPlusAPI.Windows;

    /// <summary>
    /// Main context
    /// </summary>
    public class MainViewModel : VmBase
    {
        private readonly Renga.Application _rengaApplication;
        private readonly Renga.IModelView _modelView;
        private bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            Levels = new ObservableCollection<LevelWrapper>();
            _rengaApplication = new Renga.Application();
            if (_rengaApplication.ActiveView is Renga.IModelView modelView)
            {
                _modelView = modelView;
                IsEnabled = true;
            }
        }

        /// <summary>
        /// Is plugin work enabled
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value)
                    return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Collection of <see cref="LevelWrapper"/>
        /// </summary>
        public ObservableCollection<LevelWrapper> Levels { get; }

        /// <summary>
        /// Inverse level visibility
        /// </summary>
        public ICommand InverseLevelVisibilityCommand => new RelayCommand<LevelWrapper>(level =>
        {
            try
            {
                InverseLevelVisibility(level);
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        });

        /// <summary>
        /// Isolate level
        /// </summary>
        public ICommand IsolateLevelCommand => new RelayCommand<LevelWrapper>(level =>
        {
            try
            {
                IsolateLevel(level);
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        });

        /// <summary>
        /// Select level
        /// </summary>
        public ICommand SelectLevelCommand => new RelayCommand<LevelWrapper>(level =>
        {
            try
            {
                SelectLevel(level);
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        });

        /// <summary>
        /// Get levels from project
        /// </summary>
        public void GetLevels()
        {
            if (!IsEnabled)
                return;

            Levels.Clear();
            Levels.Add(new LevelWrapper(null));

            var project = _rengaApplication.Project;
            var modelObjectCollection = project.Model.GetObjects();
            for (var i = 0; i < modelObjectCollection.Count; i++)
            {
                var modelObject = modelObjectCollection.GetByIndex(i);
                if (modelObject.ObjectType == Renga.ObjectTypes.Level)
                {
                    Levels.Add(new LevelWrapper(modelObject));
                }
            }
        }

        private void InverseLevelVisibility(LevelWrapper level)
        {
            if (level.Id == 0)
            {
                var idsToOn = new List<int>();
                var idsToOff = new List<int>();
                foreach (var l in Levels)
                {
                    if (GetLevelVisibility(l))
                    {
                        idsToOff.Add(l.Id);
                        idsToOff.AddRange(GetObjectIdsBy(EqualLevel(l.Id)));
                    }
                    else
                    {
                        idsToOn.Add(l.Id);
                        idsToOn.AddRange(GetObjectIdsBy(EqualLevel(l.Id)));
                    }

                    _modelView.SetObjectsVisibility(idsToOn.ToArray(), true);
                    _modelView.SetObjectsVisibility(idsToOff.ToArray(), false);
                }
            }
            else
            {
                SetLevelVisibility(level, !GetLevelVisibility(level));
            }
        }

        private void IsolateLevel(LevelWrapper level)
        {
            if (level.Id == 0)
            {
                var ids = Levels.Select(l => l.Id).ToList();
                foreach (var l in Levels)
                {
                    ids.AddRange(GetObjectIdsBy(EqualLevel(l.Id)));
                }

                _modelView.SetObjectsVisibility(ids.ToArray(), true);
            }
            else
            {
                var ids = GetObjectIdsBy(NotEqualLevel(level.Id))
                    .Where(i => i != level.Id)
                    .ToList();
                ids.AddRange(Levels.Where(l => l.Id != level.Id).Select(l => l.Id));
                _modelView.SetObjectsVisibility(ids.ToArray(), false);
                SetLevelVisibility(level, true);
            }
        }

        private void SelectLevel(LevelWrapper level)
        {
            List<int> ids;
            if (level.Id == 0)
            {
                ids = GetObjectIdsBy(IsLevelObject(), true).ToList();
            }
            else
            {
                ids = new List<int> { level.Id };
                ids.AddRange(GetObjectIdsBy(EqualLevel(level.Id)));
            }

            var selection = _rengaApplication.Selection;
            selection.SetSelectedObjects(ids.ToArray());
        }

        private bool GetLevelVisibility(LevelWrapper level)
        {
            return _modelView.IsObjectVisible(level.Id);
        }

        private void SetLevelVisibility(LevelWrapper level, bool isVisible)
        {
            var ids = new List<int> { level.Id };
            ids.AddRange(GetObjectIdsBy(EqualLevel(level.Id)));

            _modelView.SetObjectsVisibility(ids.ToArray(), isVisible);
        }

        private IEnumerable<int> GetObjectIdsBy(Func<object, bool> func, bool includeLevels = false)
        {
            var project = _rengaApplication.Project;
            var modelObjectCollection = project.Model.GetObjects();
            for (var i = 0; i < modelObjectCollection.Count; i++)
            {
                var obj = modelObjectCollection.GetByIndex(i);
                if (includeLevels)
                {
                    if (func.Invoke(obj) || obj.ObjectType == Renga.ObjectTypes.Level)
                    {
                        yield return obj.Id;
                    }
                }
                else
                {
                    if (func.Invoke(obj))
                    {
                        yield return obj.Id;
                    }
                }
            }
        }

        private Func<object, bool> EqualLevel(int levelId)
        {
            return obj => obj is Renga.ILevelObject levelObject &&
                          levelObject.LevelId == levelId &&
                          obj is Renga.IModelObject;
        }

        private Func<object, bool> NotEqualLevel(int levelId)
        {
            return obj => obj is Renga.ILevelObject levelObject &&
                          levelObject.LevelId != levelId &&
                          obj is Renga.IModelObject;
        }

        private Func<object, bool> IsLevelObject()
        {
            return obj => obj is Renga.ILevelObject;
        }
    }
}
