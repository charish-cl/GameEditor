namespace TEngine.UIPanel
{
    public class UIPanelLayer: UILayer<IUIPanel>
    {
        public override void Show(IUIPanel screen)
        {
            screen.Show();
        }

        public override void Show<TProps>(IUIPanel screen, TProps properties)
        {
            screen.Show(properties);
        }

        public override void Hide(IUIPanel screen)
        {
            screen.Hide();
        }
    }
}