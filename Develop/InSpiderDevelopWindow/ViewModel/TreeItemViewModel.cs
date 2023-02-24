﻿//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/4/4 17:34:05.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace InSpiderDevelopWindow
{
    public class TreeItemViewModel: ViewModelBase, ITreeItemViewModel
    {

        #region ... Variables  ...
        internal string mName="";
        private bool mIsSelected = false;
        private bool mIsExpanded = false;
        private bool mIsEdit;
        private ICommand mAddCommand;
        private ICommand mAddGroupCommand;
        private ICommand mRenameCommand;
        private ICommand mRemoveCommand;
        private ICommand mCopyCommand;
        private ICommand mPasteCommand;
        private TreeItemViewModel mParent;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public string NamePrev { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int NameAfter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int SortIndex { get { return 1; } }

        /// <summary>
        /// 
        /// </summary>

        public ICommand AddCommand
        {
            get
            {
                if(mAddCommand==null)
                {
                    mAddCommand = new RelayCommand(() => {
                        Add();
                    },()=> { return CanAddChild(); });
                }
                return mAddCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand AddGroupCommand
        {
            get
            {
                if(mAddGroupCommand==null)
                {
                    mAddGroupCommand = new RelayCommand(() => {
                        AddGroup();
                    },()=> { return CanAddGroup(); });
                }
                return mAddGroupCommand;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public ICommand CopyCommand
        {
            get
            {
                if(mCopyCommand==null)
                {
                    mCopyCommand = new RelayCommand(() => { Copy(); },()=> { return CanCopy(); });
                }
                return mCopyCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand PasteCommand
        {
            get
            {
                if(mPasteCommand==null)
                {
                    mPasteCommand = new RelayCommand(() => { Paste(); },()=> { return CanPaste(); });
                }
                return mPasteCommand;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public virtual string Name
        {
            get
            {
                return mName;
            }
            set
            {
                if (mName != value && !string.IsNullOrEmpty(value))
                {
                    string oldName = mName;
                    if(OnRename(oldName, value))
                    {
                        mName = value;
                        ParseName(value);
                        OnNameRefresh();
                    }
                }
                OnPropertyChanged("Name");
                IsEdit = false;
            }
        }


        


        /// <summary>
        /// 
        /// </summary>
        public ICommand ReNameCommand
        {
            get
            {
                if (mRenameCommand == null)
                {
                    mRenameCommand = new RelayCommand(() => {
                        IsEdit = true;
                    },()=> { return CanReName(); });
                }
                return mRenameCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand RemoveCommand
        {
            get
            {
                if (mRemoveCommand == null)
                {
                    mRemoveCommand = new RelayCommand(() => {
                        Remove();
                    },()=> { return CanRemove(); });
                }
                return mRemoveCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEdit
        {
            get
            {
                return mIsEdit;
            }
            set
            {
                if (mIsEdit != value)
                {
                    mIsEdit = value;
                }
                OnPropertyChanged("IsEdit");
            }
        }

        /// <summary>
        /// 被选中
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                if (mIsSelected != value)
                {
                    mIsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// 展开
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return mIsExpanded;
            }
            set
            {
                if (mIsExpanded != value)
                {
                    mIsExpanded = value;
                    OnIsExpended();
                }
                OnPropertyChanged("IsExpanded");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeItemViewModel Parent
        {
            get
            {
                return mParent;
            }
            set
            {
                if (mParent != value)
                {
                    mParent = value;
                    OnPropertyChanged("Parent");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual string FullName { get { return Parent != null ? Parent.FullName + "." + this.Name : this.Name; } }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ParseName(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                NameAfter = GetNumberInt(value);
                NamePrev = value.Replace(NameAfter.ToString(), "");
            }
            else
            {
                NameAfter = 0;
                NamePrev = "";
            }
        }

        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        public  int GetNumberInt(string str)
        {
            int result = -1;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(str))
                            result = int.Parse(str);
                    }
                    catch
                    {

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnNameRefresh()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnIsExpended()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ViewModelBase GetModel(ViewModelBase mode)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Add()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void AddGroup()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Copy()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Paste()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanCopy()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanPaste()
        {
            return false;
        }


        public virtual bool CanAddChild()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanAddGroup()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanReName()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Remove()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanRemove()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public virtual bool OnRename(string oldName, string newName)
        {
            return true;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }


    public class HasChildrenTreeItemViewModel:TreeItemViewModel
    {

        #region ... Variables  ...
        private System.Collections.ObjectModel.ObservableCollection<TreeItemViewModel> mChildren = new System.Collections.ObjectModel.ObservableCollection<TreeItemViewModel>();

        protected bool mIsLoaded = false;

        private ICollectionView mView;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public HasChildrenTreeItemViewModel()
        {
            InitView();
        }

        #endregion ...Constructor...

        #region ... Properties ...

        public override int SortIndex => 0;

        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<TreeItemViewModel> Children
        {
            get
            {
                return mChildren;
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Add()
        {
            base.Add();
            RefreshView();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AddGroup()
        {
            base.AddGroup();
            RefreshView();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Remove()
        {
            base.Remove();
            RefreshView();
        }

        

        /// <summary>
        /// 
        /// </summary>
        private void InitView()
        {
            mView = CollectionViewSource.GetDefaultView(mChildren);
            var des = GetSortDescription();
            if(des!=null)
            {
                mView.SortDescriptions.Clear();
                foreach(var vv in des)
                {
                    mView.SortDescriptions.Add(vv);
                }
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        public ICollectionView View
        {
            get
            {
                return mView;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual List<SortDescription> GetSortDescription()
        {
            List<SortDescription> re = new List<SortDescription>();
            re.Add(new SortDescription("SortIndex", ListSortDirection.Ascending));
            re.Add(new SortDescription("NamePrev", ListSortDirection.Ascending));
            re.Add(new SortDescription("NameAfter", ListSortDirection.Ascending));
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshView()
        {
            mView.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnNameRefresh()
        {
            RefreshView();
        }

        /// <summary>
        /// 
        /// </summary>
        public void PreLoadChildForExpend(bool value)
        {
            if (value) mChildren.Add(new TreeItemViewModel());
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnIsExpended()
        {
            if(!mIsLoaded)
            {
                mChildren.Clear();
                LoadData();
                RefreshView();
                mIsLoaded = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void LoadData()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            foreach(var vv in Children)
            {
                vv.Dispose();
            }
            base.Dispose();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

         
    public interface ITreeItemViewModel
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
