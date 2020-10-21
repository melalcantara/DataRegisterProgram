using System;
using System.Collections.Generic;
using static System.Console;

public interface IJag
{
    bool Contains(Persona p); //True: Existe; False: No existe
    bool Add(Persona p); //True: Agregado; False: Ya existe, No agregado
    bool Remove(Persona p); //True: Removido; False: No existe, No borrado
    bool Replace(Persona pOld, Persona pNew); //True: Reemplazado; False: No existe old, no se agregó new
    Persona[] toSortedArray(); //JaggedArray to Sorted Array
}

public class Jag: IJag
{
    public int MaxElements { get; }
    public int Buckets { get; }
    public Persona[][] Set { get; set; }

    public Jag (int buckets, int maxelem, List<Persona> people){
        Buckets = buckets;
        MaxElements = maxelem;

        Set = new Persona[Buckets][];

        for (int i = 0; i < Buckets; i++){
            Set[i] = new Persona[MaxElements];
        }

        ToArray(people);
    }

    public bool Contains(Persona p){
        for (int i = 0; i < Buckets; i++){
            int min = 0, max = Set[i].Length - 1;

            do{
                int mid = (min + max) / 2;

                if (Set[i][mid] != null){
                    
                    if (Set[i][mid].Cedula == p.Cedula) return true;
                    else if (Set[i][mid].Cedula == p.Cedula && Set[i][mid].LastName == p.LastName) return true;
                    else if (Set[i][mid].LastName == p.LastName && Set[i][mid].Cedula != p.Cedula) max -= 1;
                    else if (p.CompareTo(Set[i][mid]) == -1) max = mid - 1;
                    else min = mid + 1;
                } else max -= 1;
            } while(min <= max);
        }
        return false;
    }

    public bool Add(Persona p){
        if (Space(p)){
            if (!Contains(p)){
                int pos = Convert.ToInt32(Detect(p));
                int buck = Math.Abs(p.GetHashCode() % Buckets);

                for (int i = Set[buck].Length - 1; i > pos; i--)
                    Set[buck][i] = Set[buck][i - 1];

                Set[buck][pos] = p;

                if (Contains(p)) return true;
            }
        } else{
            if (!Contains(p)){
                int buck = Math.Abs(p.GetHashCode() % Buckets);
                Persona[] backup = Set[buck];
                int max = Convert.ToInt32(Set[buck].Length + Set[buck].Length * 0.50);
                Set[buck] = new Persona[max];

                for (int i = 0; i < backup.Length; i++)
                    Set[buck][i] = backup[i];

                int pos = Convert.ToInt32(Detect(p));
                for (int i = Set[buck].Length - 1; i > pos; i--)
                    Set[buck][i] = Set[buck][i - 1];

                Set[buck][pos] = p;

                if (Contains(p)) return true;
            }
        }
        return false;
    }

    public bool Remove(Persona p){
        if (Contains(p)){
            int pos = BinarySearch(p);
            int buck = Math.Abs(p.GetHashCode() % Buckets);
            Set[buck][pos] = null;
            for (int i = pos; i < Set[buck].Length - 1; i++){
                if (Set[buck][i + 1] != null)
                    Set[buck][i] = Set[buck][i + 1];
                else if (Set[buck][i + 1] == null)
                    Set[buck][i] = null;

                if (Set[buck][i] == Set[buck][i + 1])
                    Set[buck][i + 1] = null;
            }
            
            if (!Contains(p)) return true;
            else return false;
        } else{
            WriteLine();
            return false;
        }
    }

    public bool Replace(Persona pOld, Persona pNew){
        if (Exists(pOld)){
            if (pOld.Equals(pNew)){
                if (Remove(pOld)){
                    if (Add(pNew)) return true;
                    else return false;
                }
            }
            else if (!Exists(pNew)){
                if (Remove(pOld)){
                    if (Add(pNew)) return true;
                    else return false;
                }
            }
        }
        
        return false;
    }

    private bool Exists(Persona p)
    {
        for (int i = 0; i < Buckets; i++){
            int min = 0, max = Set[i].Length - 1;

            do{
                int mid = (min + max) / 2;

                if (Set[i][mid] != null){
                    if (Set[i][mid].Cedula == p.Cedula && Set[i][mid].LastName == p.LastName) return true;
                    else if (Set[i][mid].Cedula == p.Cedula && Set[i][mid].LastName != p.LastName) return true;
                    else if (Set[i][mid].LastName == p.LastName && Set[i][mid].Cedula != p.Cedula) return false;
                    else if (p.CompareTo(Set[i][mid]) == -1) max = mid - 1;
                    else min = mid + 1;
                } else max -= 1;
            } while(min <= max);
        }
        return false;
    }

    public Persona[] toSortedArray(){
        Persona[] sorted = new Persona[1];

        foreach (var line in Set){
            foreach(var p in line){
                if (p != null){
                    if (SortSpace(sorted)){
                        for (int i = 0; i < sorted.Length; i++){
                            if (sorted[i] == null){
                                sorted[i] = p;
                                break;
                            }
                        }
                    } else{
                        int max = Convert.ToInt32(sorted.Length + sorted.Length * 0.50);
                        Persona[] backup = sorted;
                        sorted = new Persona[max];

                        for (int i = 0; i < backup.Length; i++)
                            sorted[i] = backup[i];

                        for (int i = 0; i < sorted.Length; i++){
                            if (sorted[i] == null){
                                sorted[i] = p;
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (sorted.Length == 1) return sorted;

        Array.Sort(sorted);

        return sorted;
    }

    private bool SortSpace(Persona[] arr){
        foreach (var i in arr)
            if (i == null) return true;
        return false;
    }

    public int BinarySearch(Persona p){
        int buck = Math.Abs(p.GetHashCode() % Buckets);
        int min = 0, max = Set[buck].Length - 1;

        while (min <= max){
            int mid = (min + max) / 2;

            if (Set[buck][mid] != null){
                if (Set[buck][mid].Equals(p) && Set[buck][mid].LastName == p.LastName) return mid++;
                else if (!Set[buck][mid].Equals(p) && Set[buck][mid].LastName == p.LastName) max -= 1;
                else if (p.CompareTo(Set[buck][mid]) == -1) max = mid - 1;
                else min = mid + 1;
            } else max -= 1;
        }

        return -1;
    }

    private object Detect(Persona p){
        int buck = Math.Abs(p.GetHashCode() % Buckets);
        int min = 0, max = Set[buck].Length;

        while (min <= max){
            int mid = (min + max) / 2;

            if (Set[buck][mid] != null){
                if (p.CompareTo(Set[buck][mid]) == 1) return max;
                else if (p.CompareTo(Set[buck][0]) == -1) return 0;
                else if (p.CompareTo(Set[buck][mid]) == 1 && p.CompareTo(Set[buck][++mid]) == -1) return mid++;
                else if (p.CompareTo(Set[buck][mid]) == 1 && p.CompareTo(Set[buck][--mid]) == -1) return mid++;
                else if (p.CompareTo(Set[buck][mid]) == -1) max = mid - 1;
                else min = mid + 1;
            } else max -= 1;
        }

        return null;
    }

    private void ToArray(List<Persona> people){
        foreach (var i in people){
            int buck = Math.Abs(i.GetHashCode() % Buckets);
            if (Space(i)){
                if (!Contains(i)){
                    for (int j = 0; j < Set[buck].Length; j++){
                        if (Set[buck][j] == null){
                            Set[buck][j] = i;
                            break;
                        }
                    }
                }
            } else{
                if (!Contains(i)){
                    Persona[] backup = Set[buck];
                    int max = Convert.ToInt32(Set[buck].Length + Set[buck].Length * 0.50);
                    Set[buck] = new Persona[max];

                    for (int j = 0; j < backup.Length; j++){
                        Set[buck][j] = backup[j];
                    }

                    for (int j = 0; j < Set[buck].Length; j++){
                        if (Set[buck][j] == null){
                            Set[buck][j] = i;
                            break;
                        }
                    }
                }
            }
        }
    }

    private bool Space(Persona p){
        int buck = Math.Abs(p.GetHashCode() % Buckets);
        for (int i = 0; i <= Set[buck].Length - 1; i++){
            if (Set[buck][i] == null) return true; 
        }
        return false;
    }
}