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
  TuNgay: string;
  DenNgay: string;
  Nhom: string;
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
      c.forEach(x=>{
        if(x.Current){
          this.classCur = x.Name;
        }
      })
      this.GetGroup();
    })
  }
  private GetGroup() : void{
    this.service.Get<any>("/api/group").subscribe(g =>{
      this.groups = g;
      g.forEach(x=>{
        if(x.Current){
          this.Nhom = x.Value;
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
