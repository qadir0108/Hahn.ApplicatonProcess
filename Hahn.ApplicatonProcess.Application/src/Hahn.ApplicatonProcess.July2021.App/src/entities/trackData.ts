export class TrackData {
  userId: string;
  assetId: string;
  trackUntrack: boolean;
  
  public constructor(init?:Partial<TrackData>) {
    Object.assign(this, init);
  }
}
