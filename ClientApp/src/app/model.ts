export class TKB{
    name: string;
    days: Timing[];
}
export class Classes{
    Name: string;
    Current: boolean;
}
export class Week{
    Time: string;
    Number: number;
    Text: string;
    Current: boolean;
}
export class Detail {
    Phong: string;
    Ngay: string;
    SoTiet: string;
    NoiDungDay: string;
    TenGV: string;
    TenMH: string;
    TenLop: string;
    ThoiGianVaoHoc: string;
    DayOfWeek: string;
}
export class Timing{
    Time: string;
    Name: string;
    Current: boolean;
    Morning: Detail[];
    Afternoon: Detail[];
}