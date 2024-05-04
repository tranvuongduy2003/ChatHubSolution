export function SkeletonUserItemBox() {
  return (
    <div className="flex items-center w-full gap-4 p-4 rounded-lg cursor-pointer">
      <div className="w-16 h-16 rounded-full bg-slate-300 animate-pulse" />
      <div className="flex flex-col items-start justify-between gap-2 w-[calc(100%-104px)]">
        <div className="h-[18px] w-1/2 bg-slate-300 rounded-full animate-pulse"></div>
        <h4 className="w-full h-4 rounded-full bg-slate-300 animate-pulse"></h4>
      </div>
      <div className="w-2 h-2 bg-green-400 rounded-full animate-pulse"></div>
    </div>
  );
}
