using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProTeGe{
	namespace MenuLib{
		public class MenuItem : System.IComparable {

            public delegate void MenuAction();

            public string name;
            public MenuAction action;
            public MenuItem parent;

            public int CompareTo (object obj)
			{
				MenuItem other = obj as MenuItem;
				if (other == null)
					throw new System.InvalidOperationException ();
				return name.CompareTo (other.name);
			}

			public int MaxHeight {
				get { 
					int max = size;
					foreach (MenuItem x in nextLevel)
						if (x.size > max)
							max = x.size;
					return max;
				}
			}

			public void Sort ()
			{
				nextLevel.Sort ();
				foreach (MenuItem x in nextLevel)
					x.Sort ();
			}

			public int size{ get { return nextLevel.Count; } }

			public MenuItem this [string s] {
				get {
					foreach (MenuItem x in nextLevel)
						if (x.name == s)
							return x;
					return null;
					// throw new System.Exception ("menuitem \"" + name + "\" does not have child \"" + s + "\"");
				}
			}

			public MenuItem[] GetChildren ()
			{
				return nextLevel.ToArray ();
			}

			public void AddChild (MenuItem child)
			{
				nextLevel.Add (child);
				child.parent = this;
			}

			public MenuItem (string name, MenuAction action = null)
			{
				this.name = name;
				this.action = action;
				nextLevel = new List<MenuItem> ();
				parent = null;
			}

			private readonly List<MenuItem> nextLevel;
		
		}

		public abstract class MenuGenerator : MonoBehaviour {
			public void Awake(){
				GetComponent<MenuController>().root = root;
			}

			protected MenuItem root;
		}
	}
}
