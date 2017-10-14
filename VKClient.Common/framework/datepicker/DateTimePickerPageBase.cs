using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using VKClient.Common.Localization;

namespace VKClient.Common.Framework.DatePicker
{
  public abstract class DateTimePickerPageBase : PhoneApplicationPage, IDateTimePickerPage
  {
    private const string VisibilityGroupName = "VisibilityStates";
    private const string OpenVisibilityStateName = "Open";
    private const string ClosedVisibilityStateName = "Closed";
    private const string StateKey_Value = "DateTimePickerPageBase_State_Value";
    private LoopingSelector _primarySelectorPart;
    private LoopingSelector _secondarySelectorPart;
    private LoopingSelector _tertiarySelectorPart;
    private Storyboard _closedStoryboard;
    private DateTime? _value;

    public DateTime? Value
    {
      get
      {
        return this._value;
      }
      set
      {
        this._value = value;
        DateTimeWrapper dateTimeWrapper = new DateTimeWrapper(this._value.GetValueOrDefault(DateTime.Now));
        this._primarySelectorPart.DataSource.SelectedItem = dateTimeWrapper;
        this._secondarySelectorPart.DataSource.SelectedItem = dateTimeWrapper;
        this._tertiarySelectorPart.DataSource.SelectedItem = dateTimeWrapper;
      }
    }

    protected DateTimePickerPageBase()
    {
      //base.\u002Ector();
    }

    protected void InitializeDateTimePickerPage(LoopingSelector primarySelector, LoopingSelector secondarySelector, LoopingSelector tertiarySelector)
    {
      if (primarySelector == null)
        throw new ArgumentNullException("primarySelector");
      if (secondarySelector == null)
        throw new ArgumentNullException("secondarySelector");
      if (tertiarySelector == null)
        throw new ArgumentNullException("tertiarySelector");
      this._primarySelectorPart = primarySelector;
      this._secondarySelectorPart = secondarySelector;
      this._tertiarySelectorPart = tertiarySelector;
      this._primarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.OnDataSourceSelectionChanged);
      this._secondarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.OnDataSourceSelectionChanged);
      this._tertiarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.OnDataSourceSelectionChanged);
      // ISSUE: method pointer
      this._primarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler( this.OnSelectorIsExpandedChanged);
      // ISSUE: method pointer
      this._secondarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler( this.OnSelectorIsExpandedChanged);
      // ISSUE: method pointer
      this._tertiarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler( this.OnSelectorIsExpandedChanged);
      ((UIElement) this._primarySelectorPart).Visibility = Visibility.Collapsed;
      ((UIElement) this._secondarySelectorPart).Visibility = Visibility.Collapsed;
      ((UIElement) this._tertiarySelectorPart).Visibility = Visibility.Collapsed;
      int num1 = 0;
      foreach (LoopingSelector loopingSelector in this.GetSelectorsOrderedByCulturePattern())
      {
        int num2 = num1;
        Grid.SetColumn((FrameworkElement) loopingSelector, num2);
        int num3 = 0;
        ((UIElement) loopingSelector).Visibility = ((Visibility) num3);
        ++num1;
      }
      FrameworkElement child = VisualTreeHelper.GetChild((DependencyObject) this, 0) as FrameworkElement;
      if (child != null)
      {
        foreach (VisualStateGroup visualStateGroup in (IEnumerable) VisualStateManager.GetVisualStateGroups(child))
        {
          if ("VisibilityStates" == visualStateGroup.Name)
          {
            foreach (VisualState state in (IEnumerable) visualStateGroup.States)
            {
              if ("Closed" == state.Name && state.Storyboard != null)
              {
                this._closedStoryboard = state.Storyboard;
                ((Timeline) this._closedStoryboard).Completed += (new EventHandler(this.OnClosedStoryboardCompleted));
              }
            }
          }
        }
      }
      if (this.ApplicationBar != null)
      {
        this.ApplicationBar.ForegroundColor = VKConstants.AppBarFGColor;
        this.ApplicationBar.BackgroundColor = ((Application.Current.Resources["PhoneChromeBrush"] as SolidColorBrush).Color);
        foreach (object button in (IEnumerable) this.ApplicationBar.Buttons)
        {
          IApplicationBarIconButton iapplicationBarIconButton = button as IApplicationBarIconButton;
          if (iapplicationBarIconButton != null)
          {
            if ("DONE" == ((IApplicationBarMenuItem) iapplicationBarIconButton).Text)
            {
              ((IApplicationBarMenuItem) iapplicationBarIconButton).Text = CommonResources.AppBarConfirmChoice;
              ((IApplicationBarMenuItem) iapplicationBarIconButton).Click+=(new EventHandler(this.OnDoneButtonClick));
            }
            else if ("CANCEL" == ((IApplicationBarMenuItem) iapplicationBarIconButton).Text)
            {
              ((IApplicationBarMenuItem) iapplicationBarIconButton).Text = CommonResources.AppBar_Cancel;
              ((IApplicationBarMenuItem) iapplicationBarIconButton).Click+=(new EventHandler(this.OnCancelButtonClick));
            }
          }
        }
      }
      VisualStateManager.GoToState((Control) this, "Open", true);
    }

    private void OnDataSourceSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DataSource dataSource = (DataSource) sender;
      this._primarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
      this._secondarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
      this._tertiarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
    }

    private void OnSelectorIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      // ISSUE: explicit reference operation
      if (!(bool) e.NewValue)
        return;
      this._primarySelectorPart.IsExpanded = sender == this._primarySelectorPart;
      this._secondarySelectorPart.IsExpanded = sender == this._secondarySelectorPart;
      this._tertiarySelectorPart.IsExpanded = sender == this._tertiarySelectorPart;
    }

    private void OnDoneButtonClick(object sender, EventArgs e)
    {
      this._value = new DateTime?(((DateTimeWrapper) this._primarySelectorPart.DataSource.SelectedItem).DateTime);
      this.ClosePickerPage();
    }

    private void OnCancelButtonClick(object sender, EventArgs e)
    {
      this._value = new DateTime?();
      this.ClosePickerPage();
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException("e");
      e.Cancel = true;
      this.ClosePickerPage();
    }

    private void ClosePickerPage()
    {
      if (this._closedStoryboard != null)
        VisualStateManager.GoToState((Control) this, "Closed", true);
      else
        this.OnClosedStoryboardCompleted(null,  null);
    }

    private void OnClosedStoryboardCompleted(object sender, EventArgs e)
    {
      ((Page) this).NavigationService.GoBack();
    }

    protected abstract IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern();

    protected static IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern(string pattern, char[] patternCharacters, LoopingSelector[] selectors)
    {
      if (pattern == null)
        throw new ArgumentNullException("pattern");
      if (patternCharacters == null)
        throw new ArgumentNullException("patternCharacters");
      if (selectors == null)
        throw new ArgumentNullException("selectors");
      if (patternCharacters.Length != selectors.Length)
        throw new ArgumentException("Arrays must contain the same number of elements.");
      List<Tuple<int, LoopingSelector>> tupleList = new List<Tuple<int, LoopingSelector>>(patternCharacters.Length);
      for (int index = 0; index < patternCharacters.Length; ++index)
        tupleList.Add(new Tuple<int, LoopingSelector>(pattern.IndexOf(patternCharacters[index]), selectors[index]));
      return (IEnumerable<LoopingSelector>)Enumerable.Where<LoopingSelector>(Enumerable.Select<Tuple<int, LoopingSelector>, LoopingSelector>(Enumerable.OrderBy<Tuple<int, LoopingSelector>, int>(Enumerable.Where<Tuple<int, LoopingSelector>>(tupleList, (Func<Tuple<int, LoopingSelector>, bool>)(p => -1 != p.Item1)), (Func<Tuple<int, LoopingSelector>, int>)(p => p.Item1)), (Func<Tuple<int, LoopingSelector>, LoopingSelector>)(p => p.Item2)), (Func<LoopingSelector, bool>)(s => s != null));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException("e");
      base.OnNavigatedFrom(e);
      if (!("app://external/" == e.Uri.ToString()))
        return;
      this.State["DateTimePickerPageBase_State_Value"] = this.Value;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException("e");
      base.OnNavigatedTo(e);
      if (!this.State.ContainsKey("DateTimePickerPageBase_State_Value"))
        return;
      this.Value = this.State["DateTimePickerPageBase_State_Value"] as DateTime?;
      if (!((Page) this).NavigationService.CanGoBack)
        return;
      ((Page) this).NavigationService.GoBack();
    }

    public abstract void SetFlowDirection(FlowDirection flowDirection);
  }
}
