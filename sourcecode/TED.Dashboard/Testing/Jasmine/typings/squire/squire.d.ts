declare class Squire {
    mock(path, mock): Squire;
    require(dependencies: string[], callback: (...deps: any[]) => any): void;
    remove():void;
}