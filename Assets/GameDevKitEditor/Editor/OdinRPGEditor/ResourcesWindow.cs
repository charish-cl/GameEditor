using GameDevKit;

namespace GameDevKitEditor
{
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using UnityEditor;
    using UnityEngine;
    using System.Linq;

    // 
    // This is the main RPG editor that which exposes everything included in this sample project.
    // 
    // This editor window lets users edit and create characters and items. To achieve this, we inherit from OdinMenuEditorWindow 
    // which quickly lets us add menu items for various objects. Each of these objects are then customized with Odin attributes to make
    // the editor user friendly. 
    // 
    // In order to let the user create items and characters, we don't actually make use of the [CreateAssetMenu] attribute 
    // for any of our scriptable objects, instead we've made a custom ScriptableObjectCreator, which we make use of in the 
    // in the custom toolbar drawn in OnBeginDrawEditors method below.
    // 
    // Go on an adventure in various classes to see how things are achived.
    // 

    public class ResourcesWindow : OdinMenuEditorWindow
    {
        [MenuItem("FTool/ResourcesWindow")]
        private static void Open()
        {
            var window = GetWindow<ResourcesWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            // Adds the character overview table.
            // tree.Add("Characters", new CharacterTable(CharacterOverview.Instance.AllCharacters));
            //
            // // Adds all characters.
            // tree.AddAllAssetsAtPath("Characters", "Assets/Plugins/Sirenix", typeof(Character), true, true);
            //
            // // Add all scriptable object items.
            // tree.AddAllAssetsAtPath("", "Assets/Plugins/Sirenix/Demos/SAMPLE - RPG Editor/Items", typeof(Item), true)
            //     .ForEach(this.AddDragHandles);
            //
            // // Add drag handles to items, so they can be easily dragged into the inventory if characters etc...
            // tree.EnumerateTree().Where(x => x.Value as Item).ForEach(AddDragHandles);
            var scriptObjectTypes = TypeCache.GetTypesWithAttribute<ScriptObjectTreeAttribute>()
                .Where(t => typeof(ScriptableObject).IsAssignableFrom(t));
            
            foreach (var scriptObjectType in scriptObjectTypes)
            {
                var attribute = scriptObjectType.GetCustomAttributes(typeof(ScriptObjectTreeAttribute), false).FirstOrDefault() as ScriptObjectTreeAttribute;
                var typeName = scriptObjectType.Name;

                var directoryName = attribute.treedirectoryName;
                var path = attribute.path;
                //  var icon = attribute.getIcon;
                
                tree.AddAllAssetsAtPath(directoryName, path, scriptObjectType, true, false);
                //
                // if (icon != null)
                // {
                //     tree.EnumerateTree().AddIcons(x => icon(x));
                // }
            }
            
            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                var scriptObjectTypes = TypeCache.GetTypesWithAttribute<ScriptObjectTreeAttribute>();
                foreach (var scriptObjectType in scriptObjectTypes)
                {
                    var attribute = scriptObjectType.GetCustomAttributes(typeof(ScriptObjectTreeAttribute), false).FirstOrDefault() as ScriptObjectTreeAttribute;
                    if (attribute != null && attribute.isAddMenuItem)
                    {
                        var buttonText = "Create " + scriptObjectType.Name;
                        if (SirenixEditorGUI.ToolbarButton(new GUIContent(buttonText)))
                        {
                            
                            ScriptableObjectCreator.ShowDialog(attribute.path, scriptObjectType, obj =>
                            {
                                // var item = obj as INameable;
                                // if (item != null)
                                // {
                                //     item.Name = obj.name;
                                // }

                                base.TrySelectMenuItemWithObject(obj);
                            });
                        }
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

    }
}