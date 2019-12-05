import { Component } from '@angular/core';
import { ApiService } from './api.service';
import { TKB, Week, Classes } from './model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})



export class AppComponent {
  tkb: TKB[];
  week: Week[];
  classes: Classes[];
  groups : any[];
  classCur: string;
  classTmp: string;
  TuNgay: string;
  DenNgay: string;
  Nhom: string;
  NhomTmp: string;
  Setting: boolean = false;
  constructor(private service: ApiService){}

  private GetSchedule(): void {
    let param = 'Class=CÐD8I&TuNgay=18/11/2019&DenNgay=25/11/2019&Nhom=N2';
    if(this.classCur && this.TuNgay && this.DenNgay){
      param = `Class=${this.classCur}&TuNgay=${this.TuNgay}&DenNgay=${this.DenNgay}`;
      if(this.Nhom){
        param += `&Nhom=${this.Nhom}`;
      }
    }
    this.service.Get<TKB[]>('/api/Schedule?' + param).subscribe(x=>{
      this.tkb = x;
    });
  }
  onChange(e){
    const newVal = e.target.value;
    this.TuNgay = newVal;
    this.GetSchedule();
  }
  onChangeDenNgay(e){
    const newVal = e.target.value;
    this.DenNgay = newVal;
    this.GetSchedule();
  }
  onChangeClass(e){
    const newVal = e.target.value;
    this.classCur = newVal;
    this.GetSchedule();
  }
  handleSave(){
    this.Setting = false;
    if(this.classTmp){
      localStorage.setItem("class",this.classTmp);
      this.classCur = this.classTmp;
      this.classes.forEach(item=>{
        item.Current = item.Name == this.classTmp;
      });
    }
    if(this.NhomTmp){
      localStorage.setItem("group",this.NhomTmp);
      this.Nhom = this.NhomTmp;
      this.groups.forEach(item=>{
        item.Current = item.Value == this.NhomTmp;
      });
    }
    this.GetSchedule();
  }
  onSaveClass(e){
    this.classTmp = e.target.value;
    
  }
  onSaveGroup(e){
    this.NhomTmp =  e.target.value;
  }
  onChangeGroup(e){
    const newVal = e.target.value;
    this.Nhom = newVal;
    if(newVal == ""){
      this.Nhom = null;
    }
    this.GetSchedule();
  }
  private GetClass() : void{
    this.service.Get<Classes[]>("/api/class").subscribe(c =>{
      this.classes = c;
      var cl = localStorage.getItem("class");
      c.forEach(x=>{
        if(cl){
          x.Current = x.Name == cl;
          if(x.Current){
            this.classCur = cl;
            this.classTmp = cl;
          }
        }
        else{
          if(x.Current){
            this.classCur = x.Name;
            this.classTmp = x.Name;
          }
        }
      })
      this.GetGroup();
    })
  }
  private GetGroup() : void{
    this.service.Get<any>("/api/group").subscribe(g =>{
      this.groups = g;
      var gr = localStorage.getItem("group");

      g.forEach(x=>{
        if(gr){
          x.Current = x.Value == gr;
          if(x.Current){
            this.Nhom = gr;
            this.NhomTmp = gr;
          }
        } 
        else{
          if(x.Current){
            this.Nhom = x.Value;
          }
        } 
      })
      this.GetSchedule();
    });
  }
  ngOnInit() : void{
    this.service.Get<Week[]>("/api/week").subscribe(week=>{
      this.week = week;
      week.forEach(x=>{
        if(x.Current){
          this.TuNgay = x.Time;
          this.DenNgay = x.Time;
        }
      });
      this.GetClass();
    });
  }
}
